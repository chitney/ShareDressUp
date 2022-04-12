using UnityEngine;
using UnityEngine.UI;

public class UILocaleButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Image tickImage;
    [SerializeField]
    private Text text;

    Language language;

    void Awake()
    {
        button.onClick.AddListener(ChangeLanguage);
    }

    public void Init(Language _language)
    {
        language = _language;
        text.text = language.name.Locale();
        tickImage.enabled = _language.IsCurrent;
    }

    private void ChangeLanguage()
    {
        UISettings.ChangeLanguage(language);
    }
}
