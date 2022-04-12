using System.Collections;
using UnityEngine;

public class MathAnimation : MonoBehaviour
{
    [SerializeField]
    protected float Speed = 1f;
    [SerializeField]
    protected bool Loop = true;
    [SerializeField]
    protected bool PlayOnAwake = true;

    [SerializeField, HideInInspector]
    protected bool Disable = false;

    public virtual bool check_anim() { return true; }

    protected virtual void OnEnable()
    {
        if (PlayOnAwake)
            StartCoroutine("Animate");
    }

    protected virtual void OnDisable()
    {
        StopCoroutine("Animate");
    }

    protected virtual IEnumerator Animate()
    {
        yield return null;
    }
}
