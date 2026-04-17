using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class ImageAspectRatioFitter : AspectRatioFitter
{
    private Image _image;
    private Sprite _lastSprite;

    protected override void Awake()
    {
        base.Awake();
        Cache();
        UpdateAspect();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Cache();
        UpdateAspect();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        Cache();
        UpdateAspect();
    }
#endif

    private void Update()
    {
        // Detect sprite change at runtime or in editor
        if (_image == null) return;

        if (_image.sprite != _lastSprite)
        {
            UpdateAspect();
        }
    }

    private void Cache()
    {
        if (_image == null)
            _image = GetComponent<Image>();
    }

    private void UpdateAspect()
    {
        if (_image == null || _image.sprite == null)
            return;

        _lastSprite = _image.sprite;

        float width = _image.sprite.rect.width;
        float height = _image.sprite.rect.height;

        if (height == 0) return;

        aspectRatio = width / height;
    }
}