using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettings_LanguageToggle : MonoBehaviour
{
    [SerializeField]
    private UILanguageToggle UILanguageToggle;
    [DBItem(typeof(LocalisationDatabase)), SerializeField]
    private int EnLanguage;
    [DBItem(typeof(LocalisationDatabase)), SerializeField]
    private int RuLanguage;

    Language _en_language;
    Language _ru_language;

    public bool IsEn => _en_language.IsCurrent;

    private void Awake()
    {
        _en_language = (Language)LocalisationDatabase.Instance.GetById(EnLanguage);
        _ru_language = (Language)LocalisationDatabase.Instance.GetById(RuLanguage);
    }

    private void OnEnable()
    {
        UILanguageToggle.button.onClick.AddListener(SetLanguage);
        UILanguageToggle.UpdateCurrentLanguage(IsEn);
    }

    private void OnDisable()
    {
        UILanguageToggle.button.onClick.RemoveAllListeners();
    }

    private void SetLanguage()
    {
        if (IsEn)
        {
            UISettings.ChangeLanguage(_ru_language);
        }
        else UISettings.ChangeLanguage(_en_language);

        UILanguageToggle.UpdateCurrentLanguage(IsEn);
    }
}
