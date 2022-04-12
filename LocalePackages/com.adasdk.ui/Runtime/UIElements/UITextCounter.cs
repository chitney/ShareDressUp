using UnityEngine;
using UnityEngine.UI;

public class UITextCounter : UICounter
{
    [SerializeField]
    protected Text counterText;

    protected virtual void SetCounterText()
    {
        if (counterText != null)
            counterText.text = CurrentUIntValue.ToString();
    }

    public override void Init(float _startValue)
    {
        base.Init(_startValue);
        SetCounterText();
    }

    protected override void UpdateValue()
    {
        base.UpdateValue();
        SetCounterText();
    }

}
