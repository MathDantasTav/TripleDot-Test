using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_Text))]
public class TMPNumberAnimation : MonoBehaviour
{
    [SerializeField] private float[] _bumpScalePerDigit = new float[] { 1.3f };
    [SerializeField] private float[] _bumpDurationPerDigit = new float[] { 0.2f };

    [SerializeField] private bool _playOnAwake;
    [SerializeField] private int _from;
    [SerializeField] private int _to;
    [SerializeField] private float _duration;
    [SerializeField] private AnimationCurve _curve;

    [SerializeField] private bool _doFinalBounce = true;
    [SerializeField] private float _finalBounceScale = 1.4f;
    [SerializeField] private float _finalBounceDuration = 0.25f;

    public UnityEvent _onFinishAnimation;

    private TMP_Text _text;
    private Coroutine _mainRoutine;

    private Vector3[][] _baseVertices;
    private Vector3 _originalScale;

    private List<DigitAnim> _activeAnims = new List<DigitAnim>();

    private class DigitAnim
    {
        public int charIndex;
        public float time;
        public float duration;
        public float scale;
    }

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _originalScale = transform.localScale;
    }

    private void Start()
    {
        if (_playOnAwake)
            PlayDefaultValues();
    }
    
    public void PlayDefaultValues()
    {
        PlayAnimation(_from, _to, _duration);
    }

    public void PlayAnimation(int from, int to, float duration)
    {
        if (_mainRoutine != null)
            StopCoroutine(_mainRoutine);

        transform.localScale = _originalScale;
        _mainRoutine = StartCoroutine(MainRoutine(from, to, duration));
    }

    private IEnumerator MainRoutine(int from, int to, float duration)
    {
        float elapsed = 0f;
        int lastValue = from;
        _text.text = from.ToString();
        CacheMeshBaseline();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            float easedT = _curve.Evaluate(t);
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(from, to - 1, easedT));

            if (currentValue != lastValue)
            {
                string prev = lastValue.ToString();
                string next = currentValue.ToString();

                _text.text = next;
                CacheMeshBaseline();

                int changedIndex = GetChangedDigitIndex(prev, next);

                float bumpScale = _bumpScalePerDigit[Mathf.Min(changedIndex, _bumpScalePerDigit.Length - 1)];
                float bumpDuration = _bumpDurationPerDigit[Mathf.Min(changedIndex, _bumpDurationPerDigit.Length - 1)];

                if (bumpScale != 1f && bumpDuration > 0f)
                {
                    int charIndex = _text.textInfo.characterCount - changedIndex - 1;

                    _activeAnims.Add(new DigitAnim
                    {
                        charIndex = charIndex,
                        duration = bumpDuration,
                        scale = bumpScale,
                        time = 0f
                    });
                }

                lastValue = currentValue;
            }

            UpdateAnimations();
            
            yield return null;
        }

        // Ensure final value
        _text.text = to.ToString();
        CacheMeshBaseline();

        // Finish remaining animations
        while (_activeAnims.Count > 0)
        {
            UpdateAnimations();
            yield return null;
        }

        if (_doFinalBounce)
            yield return FinalBounce();
    }

    private void UpdateAnimations()
    {
        var textInfo = _text.textInfo;
        
        // Reset mesh to baseline
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var verts = textInfo.meshInfo[i].vertices;
            var baseVerts = _baseVertices[i];

            for (int j = 0; j < verts.Length; j++)
                verts[j] = baseVerts[j];
        }

        // Apply all animations
        for (int i = _activeAnims.Count - 1; i >= 0; i--)
        {
            var anim = _activeAnims[i];
            anim.time += Time.deltaTime;

            float t = anim.time / Mathf.Max(0.0001f, anim.duration);

            if (t >= 1f)
            {
                _activeAnims.RemoveAt(i);
                continue;
            }

            float scale = 1 + Mathf.Sin(t * Mathf.PI) * (anim.scale - 1);

            if (anim.charIndex < 0 || anim.charIndex >= textInfo.characterCount)
                continue;

            var charInfo = textInfo.characterInfo[anim.charIndex];
            if (!charInfo.isVisible)
                continue;

            int matIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            var verts = textInfo.meshInfo[matIndex].vertices;
            var baseVerts = _baseVertices[matIndex];

            Vector3 center =
                (baseVerts[vertexIndex + 0] +
                 baseVerts[vertexIndex + 1] +
                 baseVerts[vertexIndex + 2] +
                 baseVerts[vertexIndex + 3]) / 4f;

            for (int v = 0; v < 4; v++)
            {
                Vector3 offset = baseVerts[vertexIndex + v] - center;
                verts[vertexIndex + v] = center + offset * scale;
            }
        }

        _text.UpdateVertexData();
    }

    private IEnumerator FinalBounce()
    {
        _onFinishAnimation?.Invoke();

        float t = 0;

        Vector3 start = _originalScale;
        Vector3 peak = _originalScale * _finalBounceScale;

        while (t < 1)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, _finalBounceDuration);

            float s = Mathf.Sin(t * Mathf.PI);
            transform.localScale = Vector3.Lerp(start, peak, s);

            yield return null;
        }

        transform.localScale = _originalScale;
    }

    private void CacheMeshBaseline()
    {
        _text.ForceMeshUpdate();

        var textInfo = _text.textInfo;

        _baseVertices = new Vector3[textInfo.meshInfo.Length][];

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            _baseVertices[i] = textInfo.meshInfo[i].vertices.Clone() as Vector3[];
        }
    }

    private int GetChangedDigitIndex(string a, string b)
    {
        int offset = Mathf.Max(0, a.Length - b.Length);

        for (int i = 0; i < b.Length; i++)
        {
            int ai = i - offset;
            char ca = (ai >= 0 && ai < a.Length) ? a[ai] : ' ';

            if (ca != b[i])
                return b.Length - i - 1;
        }

        return Mathf.Max(0, b.Length - 1);
    }
}