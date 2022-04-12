using UnityEngine;

public class AudioItem : DBItem
{
    public string Name;
    public AudioClip Clip;
    public float defaultVolume = 0.75f;
    public bool EnableReplay = true;
    public bool NeedToDublicate = false;
}