using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : Controller, ISerializable
{
    [SerializeField]
    private AudioSource efxSource;
    [SerializeField]
    private AudioSource musicSource;

    public static SoundController Instance => Controllers.Get<SoundController>();

    private AudioDatabase AudioDatabase
    {
        get
        {
            if (_db == null) _db = DBManager.Get<AudioDatabase>();

            return _db;
        }
    }

    private AudioDatabase _db;

    private void Start()
    {
        if (MusicActive)
            musicSource.Play();
    }

    public JSONObject Serialize()
    {
        JSONObject obj = new JSONObject();
        obj.Add("ma", MusicActive ? 0 : 1);
        obj.Add("sa", SoundsActive ? 0 : 1);
        obj.Add("va", VibroActive ? 0 : 1);
        return obj;
    }

    public void Deserialize(JSONObject obj)
    {
        if (obj == null) return;
        MusicActive = obj["ma"] == 0;
        SoundsActive = obj["sa"] == 0;
        VibroActive = obj["va"] == 0;
    }

    public string Name()
    {
        return "soundc";
    }

    public void PlaySingle(int _id)
    {
        if (SoundsActive)
        {
            AudioItem clip = AudioDatabase.GetById(_id) as AudioItem;
            if (clip != null && clip.Clip != null)
                PlaySingle(clip);
        }
    }

    private void PlaySingle(AudioItem clip)
    {
        var audioSource = inGameItems.Find(a => a.clip == clip);
        if (audioSource != null)
        {
            if (clip.EnableReplay)
                audioSource.Play();
            else
                if (!clip.NeedToDublicate)
                return;
        }

        if (poolItems.Count > 0)
        {
            Enable(poolItems[poolItems.Count - 1], clip);
        }
        else InstantiateAudioSource(clip);
    }

    #region ON/OFF MUSIC SOUNDS

    public bool MusicActive { get; private set; } = true;
    public bool SoundsActive { get; private set; } = true;
    public bool VibroActive { get; private set; } = true;

    /// <summary>
    /// on/off sounds
    /// </summary>
    public void SetActiveSound(bool _enable)
    {
        SoundsActive = _enable;
        if (!SoundsActive)
            foreach (var sound in inGameItems)
                sound.Stop();
    }
    /// <summary>
    /// on/off bg music
    /// </summary>
    public void SetActiveMusic(bool _enable)
    {
        MusicActive = _enable;
        if (MusicActive) musicSource.Play();
        else musicSource.Stop();
    }

    /// <summary>
    /// on/off vibration
    /// </summary>
    public void SetActiveVibro(bool _enable)
    {
        VibroActive = _enable;
    }

    public void StopAll()
    {
        foreach (var sound in inGameItems)
            sound.Stop();
    }
    #endregion

    #region pool
    /// <summary>
    /// created inactive audioSource
    /// </summary>
    [SerializeField]
    private List<AudioSource> poolItems = new List<AudioSource>();
    /// <summary>
    /// created active audioSource
    /// </summary>
    private List<AudioSource> inGameItems = new List<AudioSource>();

    /// <summary>
    /// deactivate item from pool
    /// </summary>
    public void Disable(AudioSource _audio)
    {
        _audio.gameObject.SetActive(false);
        poolItems.Add(_audio);
        inGameItems.Remove(_audio);
        _audio.clip = null;
        #region UNITY_EDITOR
        _audio.gameObject.name = "pool";
        #endregion
    }

    /// <summary>
    /// activate item from pool
    /// </summary>
    public void Enable(AudioSource _audio, AudioItem _clip)
    {
        _audio.transform.SetParent(transform);
        poolItems.Remove(_audio);
        _audio.clip = _clip.Clip;
        _audio.volume = _clip.defaultVolume;
        _audio.gameObject.SetActive(true);
        _audio.Play();
        inGameItems.Add(_audio);
        #region UNITY_EDITOR
        _audio.gameObject.name = _clip.name;
        #endregion
    }
    /// <summary>
    /// Instantiate new AudioSource
    /// </summary>
    private AudioSource InstantiateAudioSource(AudioItem _clip)
    {
        var go = Instantiate(efxSource);
        Enable(go, _clip);
        return go;
    }

    #endregion
}
