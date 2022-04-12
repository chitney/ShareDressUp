using UnityEngine;


[CreateAssetMenu(fileName = "AudioDatabase", menuName = "ScriptableObjects/DB/AudioDatabase")]
public class AudioDatabase : DBBase
{
    public static AudioDatabase Instance { get { return DBManager.Get<AudioDatabase>(); } }    
}
