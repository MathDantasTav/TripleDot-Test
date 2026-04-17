using System;
using UnityEngine;

public class BottomBarButton : MonoBehaviour
{
    private static readonly int SelectedHash = Animator.StringToHash("Selected");
    private static readonly int LockedHash = Animator.StringToHash("Locked");
    private static readonly int IconHash = Animator.StringToHash("Icon");

    [HideInInspector] public BottomBarView View;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _iconAnimator;
    
    [Header("SFX Feedback")]
    [SerializeField] private SFXType _selectSfx;
    [SerializeField] private SFXType _deselectSfx;
    [SerializeField] private SFXType _lockedSfx;
    
    [HideInInspector] public int ButtonIndex;

    private bool _selected;
    public bool Selected
    {
        get { return _selected; }

        set
        {
            _selected = value;
            _animator.SetBool(SelectedHash, _selected);
        }
    }

    [SerializeField] private bool _locked = true;
    public bool Locked
    {
        get { return _locked; }
        set
        {
            _locked = value;
            _iconAnimator.SetBool(LockedHash, value);
        }
    }

    private void Start()
    {
        Locked = _locked;
    }

    public void TriggerIconAnimation()
    {
        _iconAnimator.SetTrigger(IconHash);
    }
    
    public void OnButtonPressed()
    {
        var audioManager = AudioManager.Instance;
        if (audioManager != null)
        {
            if (_selected)
                audioManager.PlaySFX(_deselectSfx);
            else
                audioManager.PlaySFX(Locked ? _lockedSfx : _selectSfx);
        }

        if (Locked)
        {
            TriggerIconAnimation();
            return;
        }
        
        View.ButtonSelected(ButtonIndex, !_selected);
    }
}
