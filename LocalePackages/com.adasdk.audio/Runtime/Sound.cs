using UnityEngine;

public class Sound : MonoBehaviour
{
    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (source.clip != null)
        {
            if (!source.isPlaying)
                SoundController.Instance.Disable(source);
        }
        else SoundController.Instance.Disable(source);

    }
}
