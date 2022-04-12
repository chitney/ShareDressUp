using UnityEngine;
using UnityEngine.UI;

public class UITextProgressBar : UITextCounter
{
    [SerializeField]
    private RectTransform pb_image;
    [SerializeField]
    private bool inverse;

    private float maxXSize;
    private Vector2 size;
    [SerializeField]
    private float maxValue;
    [SerializeField]
    private float currentX;
    void Awake()
    {
        maxXSize = pb_image.sizeDelta.x;
        size = pb_image.sizeDelta;
    }

    public void Init(float _startValue, float _maxValue)
    {
        maxValue = _maxValue;
        base.Init(_startValue);
        UpdateValue();
    }

    protected override void SetCounterText()
    {
        counterText.text = NextUIntValue.ToString() + "/" + maxValue;
    }

    protected override void UpdateValue()
    {
        base.UpdateValue();

        if (maxValue != 0)
        {
            currentX = inverse ? maxValue - CurrentValue : CurrentValue;
            size = new Vector2(maxXSize * currentX / maxValue, size.y);
        }

        pb_image.sizeDelta = size;
    }

}
