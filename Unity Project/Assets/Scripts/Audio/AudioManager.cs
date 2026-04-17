using UnityEngine;
using System.Linq;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    private const string SfxActiveKey = "SfxActive";
    private const string MusicActiveKey = "MusicActiveKey";
    
    public static AudioManager Instance;

    [SerializeField] private AudioLibrary _library;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private float _audioFadeDuration;

    public bool SfxActive
    {
        get
        {
            if (_sfxActive == null)
                _sfxActive = PlayerPrefs.GetInt(SfxActiveKey, 1) == 1;
            
            return _sfxActive.Value;
        }
        set
        {
            if (_sfxActive == value)
                return;
            
            _sfxActive = value;
            PlayerPrefs.SetInt(SfxActiveKey, value ? 1 : 0);
        }
    }
    private bool? _sfxActive = null;
    
    public bool MusicActive
    {
        get
        {
            if (_musicActive == null)
                _musicActive =  PlayerPrefs.GetInt(MusicActiveKey, 1) == 1;
            
            return _musicActive.Value;
        }
        set
        {
            if (_musicActive == value)
                return;
            
            _musicActive = value;
            PlayerPrefs.SetInt(MusicActiveKey, value ? 1 : 0);
            FadeAudioSource(_musicSource, value);
        }
    }
    private bool? _musicActive = null;

    private void Awake()
    {
        Instance = this;
    }

    private void FadeAudioSource(AudioSource source, bool value)
    {
        float currentVolume = source.volume;
        float finalVolume = value ? 1 : 0;
        
        DOTween.Kill(source);
        DOTween.To(
            () => currentVolume,
            x => {
                currentVolume = x;
                source.volume = currentVolume;
            },
            finalVolume,
            _audioFadeDuration
        );
    }

    public void PlaySFX(SFXType type)
    {
        if (SfxActive == false || type == SFXType.None || _library == null) return;
        
        var entry = _library.sfx.FirstOrDefault(s => s.type == type);
        if (entry != null)
        {
            _sfxSource.PlayOneShot(entry.clip, entry.volume);
        }
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (MusicActive == false) return;
        
        _musicSource.clip = clip;
        _musicSource.volume = volume;
        _musicSource.loop = true;
        _musicSource.Play();
    }
}