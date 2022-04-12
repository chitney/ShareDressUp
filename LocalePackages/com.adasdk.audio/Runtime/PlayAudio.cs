using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [DBItem(typeof(AudioDatabase)), SerializeField]
    private int id;

    public void Play()
    {
        SoundController.Instance.PlaySingle(id);
    }

}
