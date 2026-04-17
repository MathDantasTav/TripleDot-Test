using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    None,
    CrowdCheers,
    DrumRoll,
    SmallSuccess,
    Success,
    UIClickSound,
    UIOff,
    UIOn,
    UIPopSound,
    UIPositiveSound,
    Whoosh,
    UIPopSound2,
    UILockedButton,
    UIUnlockDoor
}

public enum MusicType
{
    MainLoop
}

[System.Serializable]
public class SFXEntry
{
    public SFXType type;
    public AudioClip clip;
    public float volume = 1f;
}

[CreateAssetMenu(menuName = "Scriptable Objects/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    public List<SFXEntry> sfx;
}