using System.Collections;
using UnityEngine;

public class ParticleUpdate : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle;

    void OnEnable()
    {
        Invoke("StartCkeck", 1f);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void StartCkeck()
    {
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        while (particle.isPlaying)
            yield return null;

        gameObject.SetActive(false);
    }
}
