using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private bool _playOnEnabled;
    [SerializeField] private SFXType _sfxType;

    private void OnEnable()
    {
        if (_playOnEnabled)
            PlaySfx();
    }

    public void PlaySfx()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(_sfxType);
    }
}