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
    [SerializeField] private RectTransform _baseRect;
    [SerializeField] private bool _compensateOnWidth;
    [SerializeField] private bool _compensateOnHeight;
    private float _thinResolutionCompensation => 1 / _thinResolutionScaleFactor;
    private RectTransform CanvasRect
    {
        get
        {
            if (_canvasRect == null) 
            {
                var rect = GetComponent<RectTransform>();
                if (rect == null) return null;

                var canvas = rect.GetComponentInParent<Canvas>();
                if (canvas == null) return rect;

                _canvasRect = canvas.GetComponent<RectTransform>();
            }

            return _canvasRect;
        }
    }
    private RectTransform _canvasRect;
    
#if UNITY_EDITOR
    private Vector2 _lastCanvasSize;
#endif

    void Start()
    {
        ApplyResolutionScaling();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (_lastCanvasSize.x != CanvasRect.rect.width ||
            _lastCanvasSize.y != CanvasRect.rect.height)
        {
            ApplyResolutionScaling();
        }
    }
#endif
    
    void ApplyResolutionScaling()
    {
#if UNITY_EDITOR
        _lastCanvasSize = new Vector2(CanvasRect.rect.width, CanvasRect.rect.height);
#endif

        if (_baseRect == null || _compensateRect == null)
            return;
        
        Canvas.ForceUpdateCanvases();
        
        var offsetMin = _compensateRect.offsetMin;
        var offsetMax = _compensateRect.offsetMax;
        
        float ratio = CanvasRect.rect.x / CanvasRect.rect.y;
        
        if (ratio > _aspectRatioThreshold)
        {
            _compensateRect.localScale = Vector3.one;
            
            if (_compensateOnWidth)
            {
                offsetMin.x = _baseRect.offsetMin.x;
                offsetMax.x = _baseRect.offsetMax.x;
            }

            if (_compensateOnHeight)
            {
                offsetMin.y = _baseRect.offsetMin.y;
                offsetMax.y = _baseRect.offsetMax.y;
            }
            
            _compensateRect.offsetMin = offsetMin;
            _compensateRect.offsetMax = offsetMax;
            
            return;
        }

        _compensateRect.localScale = Vector3.one * _thinResolutionScaleFactor;
        
        if (_compensateOnWidth)
        {
            float delta = (_thinResolutionCompensation - 1f) / 2;

            float width = _baseRect.rect.width;

            float expand = width * delta;

            offsetMin.x = _baseRect.offsetMin.x - expand;
            offsetMax.x = _baseRect.offsetMax.x + expand;
        }

        if (_compensateOnHeight)
        {
            float delta = (_thinResolutionCompensation - 1f) / 2;

            float height = _baseRect.rect.height;

            float expand = height * delta;

            offsetMin.y = _baseRect.offsetMin.y - expand;
            offsetMax.y = _baseRect.offsetMax.y + expand;
        }

        _compensateRect.offsetMin = offsetMin;
        _compensateRect.offsetMax = offsetMax;
    }
}
