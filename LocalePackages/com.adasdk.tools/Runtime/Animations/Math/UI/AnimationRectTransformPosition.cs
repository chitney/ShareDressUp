using System.Collections;
using UnityEngine;

public class AnimationRectTransformPosition : MathAnimation
{
    [SerializeField]
    private Vector2 start;
    [SerializeField]
    private Vector2 end;

    private RectTransform _myTransform;
    private Vector2 target;
    protected override void OnEnable()
    {
        _myTransform = transform as RectTransform;
        _myTransform.anchoredPosition = start;
        base.OnEnable();
    }

    public override bool check_anim()
    {
        if (Loop)
        {
            return true;
        }
        else return _myTransform.anchoredPosition != end;
    }

    protected override IEnumerator Animate()
    {
        while (check_anim())
        {
            if (_myTransform.anchoredPosition == start) target = end;
            else if (_myTransform.anchoredPosition == end) target = start;

            _myTransform.anchoredPosition = Vector2.MoveTowards(_myTransform.anchoredPosition, target, Speed);
            yield return null;
        }
        if (Disable) gameObject.SetActive(false);
    }

}
