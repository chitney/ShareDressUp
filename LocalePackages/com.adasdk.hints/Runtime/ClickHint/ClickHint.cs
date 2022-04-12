using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHint : Hint
{
    [SerializeField]
    private Animator animator;



    GameObject go_target;
    Vector3 target;

    public void Init(GameObject go_target)
    {
        this.go_target = go_target;
        Init(GetPosition(go_target));
    }

    public void Init(Vector3 target)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            this.target = target;
            animator.SetBool(_animator_OnClick, true);
            StartCoroutine(WaitOfEndAction());
        }
    }


    public override void Stop()
    {
        animator.SetBool(_animator_OnClick, false);
    }

    protected override void FrameUptade()
    {
        if (go_target != null)
        {
            target = GetPosition(go_target);
        }

        transform.localPosition = target;
    }

}
