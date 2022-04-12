using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILanguageToggle : MonoBehaviour
{
    [SerializeField]
    private Color activeColor;
    [SerializeField]
    private Color disableColor;
    [SerializeField]
    private Text EnText;
    [SerializeField]
    private Text RuText;
    [SerializeField]
    public Button button;

    public void UpdateCurrentLanguage(bool IsEn)
    {
        EnText.color = IsEn ? activeColor : disableColor;
        RuText.color = IsEn ? disableColor : activeColor;
    }
}
