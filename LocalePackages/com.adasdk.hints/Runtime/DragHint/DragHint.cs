using System.Collections;
using UnityEngine;

public class DragHint : Hint
{
    [SerializeField]
    private Animator animator;

    private GameObject go_from;
    private GameObject go_to;

    private Vector3 from;
    private Vector3 to;

    Coroutine _move_coroutine;

    bool onDragStart = false;

    public void Init(GameObject from, GameObject to)
    {
        if (!gameObject.activeSelf)
        {
            this.go_from = from;
            this.go_to = to;
            On();
        }
    }

    public void Init(Vector3 from, Vector3 to)
    {
        if (!gameObject.activeSelf)
        {
            this.from = from;
            this.to = to;
            On();
        }
    }

    private void On()
    {
        gameObject.SetActive(true);
        animator.SetTrigger(_animator_Show);
        animator.SetBool(_animator_OnDrag, true);
        onDragStart = false;
        InputController.OnDragStart += OnDragStart;
        StartCoroutine(WaitOfEndAction());

    }

    private void OnDragStart()
    {
        onDragStart = true;
    }

    public override void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        if (_move_coroutine!=null)
            StopCoroutine(_move_coroutine);
        _move_coroutine = null;
        animator.SetBool(_animator_OnDrag, false);
        animator.SetTrigger(_animator_Hide);
    }

    public void StartMoving()
    {
        if (_move_coroutine == null)
        {
            if (go_from != null && go_to != null)
            {
                from = go_from.transform.position;
            }

            transform.position = from;
            _move_coroutine = StartCoroutine(Move());
            
        }
    }

    public void MoveToStart()
    {
        if (go_from != null)
            from = GetPosition(go_from);
        

        transform.position = from;
        bool isAnimOn = animator.GetBool(_animator_OnDrag);
        if (!isAnimOn)
            gameObject.SetActive(false);
    }


    private IEnumerator Move()
    {
        float t = 0;

        if (go_from != null && go_to != null)
        {
            from = GetPosition(go_from);
            to = GetPosition(go_to);
        }

        transform.position = from;
        while (Vector2.Distance(transform.position, to) > 1f)
        {
            transform.position = Vector3.Lerp(from, to, t += Time.deltaTime);
            yield return null;
        }
        End();
    }

    private void End()
    {
        animator.SetTrigger(_animator_Hide);
        _move_coroutine = null;
    }

    public override bool EndAction()
    {
        return onDragStart;
    }

}
