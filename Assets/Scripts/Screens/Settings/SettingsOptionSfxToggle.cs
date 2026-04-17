using UnityEngine;

public class SettingsOptionSfxToggle : SettingsOptionToggle
{
    protected override bool GetState()
    {
        return AudioManager.Instance.SfxActive;
    }

    protected override void SetState(bool value)
    {
        AudioManager.Instance.SfxActive = value;
    }
}
