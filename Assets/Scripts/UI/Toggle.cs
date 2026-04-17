using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Toggle : MonoBehaviour
{
    public enum Type
    {
        SpriteSwap,
        ColorSwap
    }
    
    [SerializeField] private Image _targetImage;
    [SerializeField] private Type _toggleType;
    
    [SerializeField] private Color _onColor = Color.white;
    [SerializeField] private Color _offColor = Color.darkGray;
    [SerializeField] private float _duration = 0.2f;
    
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;

    [Space(10)] [Header("SFX Feedback")]
    [SerializeField]  private bool _doSfxFeedback = true;
    [SerializeField] private SFXType _onSfxType = SFXType.UIOn;
    [SerializeField] private SFXType _offSfxType = SFXType.UIOff;

    [Space(10)] 
    [SerializeField] private bool _currentState;
    public bool CurrentState => _currentState;
    public UnityEvent<bool> OnStateChanged;
    
    private Button _button;
    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);

        SetState(_currentState, true);
    }
    
    void OnClick()
    {
        SetState(!_currentState);
    }

    public void SetState(bool value, bool instant = false)
    {
        switch (_toggleType)
        {
            case Type.SpriteSwap:
                _targetImage.sprite = value ? _onSprite : _offSprite;
                break;
                
            case Type.ColorSwap:
                Color targetColor = value ? _onColor : _offColor;

                if (instant)
                {
                    _targetImage.color = targetColor;
                }
                else
                {
                    DOTween.To(
                        () => _targetImage.color,
                        x => _targetImage.color = x,
                        targetColor,
                        _duration
                    ).SetEase(Ease.OutQuad);
                }
                break;
        }
        
        if (_currentState != value)
            OnStateChanged.Invoke(value);
        
        _currentState = value;
        
        if (_doSfxFeedback && instant == false)
            AudioManager.Instance.PlaySFX(_currentState ? _onSfxType :  _offSfxType);
    }
}
