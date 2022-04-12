using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public static readonly int _animator_OnDrag = Animator.StringToHash("OnDrag");
    public static readonly int _animator_OnClick = Animator.StringToHash("OnClick");
    public static readonly int _animator_Show = Animator.StringToHash("Show");
    public static readonly int _animator_Hide = Animator.StringToHash("Hide");
    protected Vector3 GetPosition(GameObject target)
    {
        return target.layer == 5 ? target.transform.position : Camera.main.WorldToScreenPoint(target.transform.position);
    }

    protected virtual void FrameUptade(){}

    public virtual void Stop(){}

    public virtual bool EndAction() { return Input.GetMouseButton(0); }

    protected IEnumerator WaitOfEndAction()
    {
        while (!EndAction())
        {
            FrameUptade();
            yield return null;
        }
        Stop();
    }


}
