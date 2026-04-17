using UnityEngine;

public abstract class SettingsOptionToggle : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    
    private void OnEnable()
    {
        if (_toggle == null)
        {
            Debug.LogError($"{nameof(SettingsOptionToggle)} missing Toggle", this);
            return;
        }
        
        _toggle.SetState(GetState(), true);
        _toggle.OnStateChanged.AddListener(OnStateChanged);
    }

    private void OnDisable()
    {
        _toggle.OnStateChanged.RemoveListener(OnStateChanged);
    }

    private void OnStateChanged(bool value)
    {
        SetState(value);
    }
    
    protected abstract bool GetState();
    protected abstract void SetState(bool value);
}
