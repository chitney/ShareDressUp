using System.Collections;
using UnityEngine;

public class AnimationTransformScale : MathAnimation
{
    [SerializeField]
    private Vector3 start;
    [SerializeField]
    private Vector3 end;

    private RectTransform _myTransform;
    private Vector3 target;
    protected override void OnEnable()
    {
        _myTransform = transform as RectTransform;
        _myTransform.localScale = start;
        target = end;
        base.OnEnable();
    }

    public override bool check_anim()
    {
        if (Loop)
            return true;
        else
            return _myTransform.localScale != target;
    }

    protected override IEnumerator Animate()
    {
        while (check_anim())
        {
            if (_myTransform.localScale == start) target = end;
            else if (_myTransform.localScale == end) target = start;

            _myTransform.localScale = Vector2.MoveTowards(_myTransform.localScale, target, Speed);
            yield return null;
        }
    }

    public void ScaleToEnd()
    {
        target = end;
        Loop = false;
        Disable = false;
        StopAllCoroutines();
        StartCoroutine("Animate");
    }


    public void ScaleToStart()
    {
        target = start;
        Loop = false;
        Disable = false;
        StopAllCoroutines();
        StartCoroutine("Animate");
    }

}
