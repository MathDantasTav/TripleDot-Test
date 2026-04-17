using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class BtnFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Feedback")]
    [SerializeField] private List<RectTransform> _scaleFeedback;
    [SerializeField] private float _scaleFeedbackDuration = 0.1f;
    [Header("Button Press")]
    [SerializeField] private float _scalePressFeedbackIntensity = 0.9f;
    [Header("Button Hover")]
    [SerializeField] private float _scaleHoverFeedbackIntensity = 0.95f;
    
    [Space(10)]
    [Header("Color Feedback")]
    [SerializeField] private List<Image> _colorFeedback;
    [SerializeField] private Color _colorFeedbackDefaultColor = Color.white;
    [SerializeField] private float _colorFeedbackDuration = 0.1f;
    [Header("Button Press")]
    [SerializeField] private Color _colorPressFeedbackPressedColor = Color.darkGray;
    [Header("Button Hover")]
    [SerializeField] private Color _colorHoverFeedbackPressedColor = Color.lightGray;

    [Space(10)] [Header("SFX Feedback")] 
    [SerializeField] private bool _sfxFeedback = true;
    [SerializeField] private SFXType _sfxFeedbackType = SFXType.UIClickSound;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        ApplyFeedback(_scalePressFeedbackIntensity, _colorPressFeedbackPressedColor);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ApplyFeedback(1, _colorFeedbackDefaultColor);
        
        if (_sfxFeedback)
        {
            AudioManager.Instance.PlaySFX(_sfxFeedbackType);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ApplyFeedback(_scaleHoverFeedbackIntensity, _colorHoverFeedbackPressedColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ApplyFeedback(1, _colorFeedbackDefaultColor);
    }

    void ApplyFeedback(float finalScaleIntensity, Color finalColor)
    {
        foreach(var scaleFeedback in _scaleFeedback)
        {
            scaleFeedback.DOKill();
            scaleFeedback.DOScale(Vector3.one * finalScaleIntensity, _scaleFeedbackDuration);
        }
        
        foreach(var colorFeedback in _colorFeedback)
        {
            colorFeedback.DOKill();
            DOTween.To(
                () => colorFeedback.color,
                x => colorFeedback.color = x,
                finalColor,
                _colorFeedbackDuration
            );
        }
    }
}
