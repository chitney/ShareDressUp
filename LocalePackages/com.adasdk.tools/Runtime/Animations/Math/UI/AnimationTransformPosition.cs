using System.Collections;
using UnityEngine;

public class AnimationTransformPosition : MathAnimation
{
    [SerializeField]
    private Vector3 start;
    [SerializeField]
    private Vector3 end;

    private Transform _myTransform;
    private Vector3 target;
    protected override void OnEnable()
    {
        _myTransform = transform;
        base.OnEnable();
    }

    public override bool check_anim()
    {
        if (Loop)
        {
            return true;
        }
        else return _myTransform.position != end;
    }

    protected override IEnumerator Animate()
    {
        while (check_anim())
        {
            if (_myTransform.position == start) target = end;
            else if (_myTransform.position == end) target = start;

            _myTransform.position = Vector2.Lerp(_myTransform.position, target, Speed);
            yield return null;
        }
        if (Disable) gameObject.SetActive(false);
    }

}
