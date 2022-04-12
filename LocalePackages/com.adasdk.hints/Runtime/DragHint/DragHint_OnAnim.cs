using UnityEngine;

public class DragHint_OnAnim : StateMachineBehaviour
{
    private DragHint hand;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hand == null)
            hand = animator.GetComponentInParent<DragHint>();
        hand.StartMoving();
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
}
