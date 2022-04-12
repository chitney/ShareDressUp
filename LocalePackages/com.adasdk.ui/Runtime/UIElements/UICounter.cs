using System.Collections;
using UnityEngine;

/// <summary>
/// The element stores the float value and changes the current value in increments
/// </summary>
public class UICounter : MonoBehaviour
{
    [SerializeField]
    private float step = 1f;
    [SerializeField]
    private float _nextValue = 0;
    [SerializeField]
    private float _currentValue = 0;

    public float CurrentValue
    {
        get
        {
            return _currentValue;
        }
    }

    public uint CurrentUIntValue
    {
        get
        {
            return (uint)System.Math.Round(_currentValue);
        }
    }

    public uint NextUIntValue
    {
        get
        {
            return (uint)System.Math.Round(_nextValue);
        }
    }


    public virtual void Init(float _startValue)
    {
        _nextValue = _startValue;
        _currentValue = _startValue;
    }

    public virtual void SetNewValue(float _newValue)
    {
        _nextValue = _newValue;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(CheckValue());
    }

    protected virtual void OnDisable()
    {
        StopCoroutine(CheckValue());
    }

    private WaitForSeconds second = new WaitForSeconds(1f);

    private IEnumerator CheckValue()
    {
        while (gameObject.activeSelf)
        {
            while (Mathf.Abs(_currentValue - _nextValue) >= step)
            {
                UpdateValue();
                yield return null;
            }
            yield return null;
        }
    }

    protected virtual void UpdateValue()
    {
        float diff = _currentValue - _nextValue;

        float _step = step;

        if (System.Math.Abs(diff) > 100)
        {
            if (System.Math.Abs(diff) > 1000)
                _step = 100;
            else
                _step = 10;
        }

        if (diff > step)
        {
            _currentValue -= _step;
        }
        else if (diff < step)
            _currentValue += _step;
    }
}
