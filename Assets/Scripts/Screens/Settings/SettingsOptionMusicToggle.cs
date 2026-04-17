using UnityEngine;

public class SettingsOptionMusicToggle : SettingsOptionToggle
{
    protected override bool GetState()
    {
        return AudioManager.Instance.MusicActive;
    }

    protected override void SetState(bool value)
    {
        AudioManager.Instance.MusicActive = value;
    }
}
