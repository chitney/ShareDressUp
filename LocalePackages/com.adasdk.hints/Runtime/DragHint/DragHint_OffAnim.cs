using UnityEngine;

public class DragHint_OffAnim : StateMachineBehaviour
{
    private DragHint hand;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hand==null)
            hand = animator.GetComponentInParent<DragHint>();

        if (animator.GetBool(Hint._animator_OnClick))
            return;

        hand.MoveToStart();
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
}
