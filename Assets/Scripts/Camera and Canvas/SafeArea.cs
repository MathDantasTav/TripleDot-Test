using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class SafeAreaPanel : MonoBehaviour
{
    private RectTransform _rectTransform;

#if UNITY_EDITOR
    private Vector2 _lastScreenSize;
    private Rect _lastSafeArea;
#endif
    
    void OnEnable()
    {
        _rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

#if UNITY_EDITOR
    // Only run in editor (avoid per-frame cost in play mode if you want)
    void Update()
    {
        if (_lastScreenSize.x != Screen.width ||
            _lastScreenSize.y != Screen.height ||
            _lastSafeArea != Screen.safeArea)
        {
            ApplySafeArea();
        }
    }
#endif

    private void OnRectTransformDimensionsChange()
    {
        ApplySafeArea();
    }
    
    void ApplySafeArea()
    {
        if (Screen.width == 0 || Screen.height == 0)
            return;
        
        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;

        // Reset offsets so it fully fits anchors
        _rectTransform.offsetMin = Vector2.zero;
        _rectTransform.offsetMax = Vector2.zero;

#if UNITY_EDITOR
        _lastScreenSize = new Vector2(Screen.width, Screen.height);
        _lastSafeArea = safeArea;
#endif
    }
}