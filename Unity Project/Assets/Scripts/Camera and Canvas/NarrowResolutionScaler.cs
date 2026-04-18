using System;
using UnityEngine;

/// <summary>
/// Used to fix sizing on super narrow phones like the ZFold
/// Scales down the image, than resizes a specifi axis (generally the X axis)
/// in order to give it more width to work with
/// </summary>
[ExecuteAlways]
public class NarrowResolutionScaler : MonoBehaviour
{
    [SerializeField] private float _aspectRatioThreshold = 0.5f;
    [SerializeField] private float _thinResolutionScaleFactor = 0.8f;

    // Compensate Rect is used for a rect that is meant to go to the edge of the screen (i.e. a background)
    // Putting it here will make sure it still stretches to the end of the screen even when the object has been scaled down
    [SerializeField] private RectTransform _compensateRect;
    [SerializeField] private bool _compensateOnWidth;
    [SerializeField] private bool _compensateOnHeight;
    [SerializeField] private Vector2 _originalOffsetMin;
    [SerializeField] private Vector2 _originalOffsetMax;
    private float _thinResolutionCompensation => 1 / _thinResolutionScaleFactor;
    
#if UNITY_EDITOR
    private Vector2 _lastScreenSize;
    private Rect _lastSafeArea;
#endif

    void Start()
    {
        ApplyResolutionScaling();
    }

#if UNITY_EDITOR
    // Only run in editor (avoid per-frame cost in play mode if you want)
    void Update()
    {
        if (_lastScreenSize.x != Screen.width ||
            _lastScreenSize.y != Screen.height ||
            _lastSafeArea != Screen.safeArea)
        {
            ApplyResolutionScaling();
        }
    }
#endif

    void OnValidate()
    {
        ApplyResolutionScaling();
    }
    
    void ApplyResolutionScaling()
    {
#if UNITY_EDITOR
        _lastScreenSize = new Vector2(Screen.width, Screen.height);
        _lastSafeArea = Screen.safeArea;
#endif
        
        Canvas.ForceUpdateCanvases();
        
        var safeArea = Screen.safeArea;
        float ratio = safeArea.width / safeArea.height;

        var rect = GetComponent<RectTransform>();
        if (rect == null) return;
        
        if (_compensateRect == null)
            _compensateRect = rect;
        
        var offsetMin = _compensateRect.offsetMin;
        var offsetMax = _compensateRect.offsetMax;
        
        if (ratio > _aspectRatioThreshold)
        {
            rect.localScale = Vector3.one;
            
            if (_compensateOnWidth)
            {
                offsetMin.x = _originalOffsetMin.x;
                offsetMax.x = _originalOffsetMax.x;
            }

            if (_compensateOnHeight)
            {
                offsetMin.y = _originalOffsetMin.y;
                offsetMax.y = _originalOffsetMax.y;
            }
            
            _compensateRect.offsetMin = offsetMin;
            _compensateRect.offsetMax = offsetMax;
            
            return;
        }

        rect.localScale = Vector3.one * _thinResolutionScaleFactor;

        if (_compensateOnWidth)
        {
            float delta = (_thinResolutionCompensation - 1f) / 2;

            float width = _compensateRect.rect.width;

            float expand = width * delta;

            offsetMin.x = _originalOffsetMin.x - expand;
            offsetMax.x = _originalOffsetMax.x + expand;
        }

        if (_compensateOnHeight)
        {
            float delta = (_thinResolutionCompensation - 1f) / 2;

            float height = _compensateRect.rect.height;

            float expand = height * delta;

            offsetMin.y = _originalOffsetMin.y - expand;
            offsetMax.y = _originalOffsetMax.y + expand;
        }

        _compensateRect.offsetMin = offsetMin;
        _compensateRect.offsetMax = offsetMax;
    }
}
