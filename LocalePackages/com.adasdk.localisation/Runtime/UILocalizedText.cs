using UnityEngine;
using UnityEngine.UI;
public static class LocaleExtension
{
    static LocalisationDatabase db;
    public static string Locale(this string key)
    {
        if (db == null)
            db = DBManager.Get<LocalisationDatabase>();
        return db.GetByKey(key);
    }
}

public class UILocalizedText : MonoBehaviour
{
    public string Key;
    private Text Text;
    public void Start()
    {
        Text = GetComponent<Text>();
        if (Text != null)
        {
            var locale = Key.Replace(" ", "").Locale();
            if (locale != "")
            {
                Text.text = locale;
                LocalisationsController.Instance.OnLanguageChanged += OnLanguageChanged;
            }
        }
#if UNITY_EDITOR
        else
        {
            Debug.LogError("Text is null " + gameObject.name);
        }
#endif

    }

    private void OnLanguageChanged()
    {
        Text.text = Key.Locale();
    }
}
