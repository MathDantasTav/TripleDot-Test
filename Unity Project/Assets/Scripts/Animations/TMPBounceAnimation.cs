using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TMPBounceAnimation : MonoBehaviour
{
    private TMP_Text _tmp;
    private TMP_TextInfo _textInfo;

    [SerializeField] private float _amplitude = 10f;
    [SerializeField] private float _frequency = 5f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _clamp = 0f;

    void Awake()
    {
        _tmp = GetComponent<TMP_Text>();
    }

    void Update()
    {
        _tmp.ForceMeshUpdate();
        _textInfo = _tmp.textInfo;

        for (int i = 0; i < _textInfo.characterCount; i++)
        {
            var charInfo = _textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3[] vertices = _textInfo.meshInfo[meshIndex].vertices;

            float y = Mathf.Sin(Time.time * _speed + i * _frequency);
            float clampedY = Mathf.Clamp(y, _clamp, 1);
            float adjustedY = Mathf.Clamp01(Mathf.InverseLerp(_clamp, 1, clampedY));
            
            Vector3 offset = new Vector3(
                0,
                adjustedY * _amplitude,
                0
            );

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        // push changes back
        for (int i = 0; i < _textInfo.meshInfo.Length; i++)
        {
            var meshInfo = _textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            _tmp.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}