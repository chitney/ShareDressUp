using System.Collections;
using UnityEngine;

public class AnimationRectTransformSize : MathAnimation
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
        _myTransform.sizeDelta = start;
        base.OnEnable();
    }

    protected override IEnumerator Animate()
    {
        while (true)
        {
            if (_myTransform.sizeDelta == start) target = end;
            else if (_myTransform.sizeDelta == end) target = start;

            _myTransform.sizeDelta = Vector2.MoveTowards(_myTransform.sizeDelta, target, Speed);
            yield return null;
        }
    }
}
