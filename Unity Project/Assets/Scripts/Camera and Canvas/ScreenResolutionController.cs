using System;
using UnityEngine;

[ExecuteAlways]
public class ScreenResolutionController : MonoBehaviour
{
    [SerializeField] private Camera _mainCam;
    [SerializeField] private Camera _blurCam;
    [SerializeField, Tooltip(
         "Conversion factor from world units (camera size) to canvas units. " +
         "Controls how large the world-space canvas appears on screen. " +
         "Higher values make the UI appear larger (more pixels per world unit)."
     )] private float _pixelsPerWorldUnit;
    
    [Header("Blur effect")]
    [SerializeField] private Material _uiBlurMaterial;
    [SerializeField, Tooltip(
         "How much to downsample the blur render texture. " +
         "2 = half resolution, 4 = quarter resolution, etc. Higher values improve performance but reduce quality."
     )] private int _blurRenderTextureDownsampleFactor = 4;
    
    private RenderTexture _blurRenderTexture;
    private Vector2 _canvasSize;
    private RectTransform _rect;
    
#if UNITY_EDITOR
    private Vector2Int _lastScreenSize;
#endif

    private void Awake()
    {
        ApplyScaling();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Screen.width != _lastScreenSize.x || Screen.height != _lastScreenSize.y)
            ApplyScaling();
    }
#endif

    void ApplyScaling()
    {
        _rect = GetComponent<RectTransform>();
        
        if (!_mainCam || !_rect || !_blurCam) return;
        
        float height = _mainCam.orthographicSize * 2f;
        float width = height * _mainCam.aspect;

        if (_blurRenderTexture != null)
        {
            _blurCam.targetTexture = null;
            DestroyImmediate(_blurRenderTexture);
        }
        
        int rtWidth = Mathf.CeilToInt(Screen.width / _blurRenderTextureDownsampleFactor);
        int rtHeight = Mathf.CeilToInt(Screen.height / _blurRenderTextureDownsampleFactor);
        _blurRenderTexture = new RenderTexture(rtWidth, rtHeight, 0, RenderTextureFormat.ARGB32);
        _blurRenderTexture.Create();
        _blurCam.targetTexture = _blurRenderTexture;
        _uiBlurMaterial.mainTexture = _blurRenderTexture;
        
        var newSize = new Vector2(width, height) * _pixelsPerWorldUnit;

        if (newSize == _canvasSize)
        {
            return;
        }

        _canvasSize = newSize;
        _rect.sizeDelta = _canvasSize;
        
        transform.position = _mainCam.transform.position + _mainCam.transform.forward * 10f;
        transform.rotation = _mainCam.transform.rotation;
        
#if UNITY_EDITOR
        _lastScreenSize = new Vector2Int(Screen.width, Screen.height);
#endif
    }
}