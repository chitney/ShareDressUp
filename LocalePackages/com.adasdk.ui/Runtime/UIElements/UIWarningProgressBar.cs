using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWarningProgressBar : UITextProgressBar
{
    [SerializeField]
    private uint boundaryValue = 7;

    [SerializeField]
    private Animation warningAnimation;

    private uint prevValue = 0;

    protected override void UpdateValue()
    {
        base.UpdateValue();
        if (CurrentUIntValue >= boundaryValue )
        {
            if (CurrentUIntValue > prevValue) 
                Warning();
        }
        else
        {
            warningAnimation.Play("empty");
        }
        prevValue = CurrentUIntValue;

    }

    protected void Warning()
    {
        warningAnimation.Play("warning");
    }

}
