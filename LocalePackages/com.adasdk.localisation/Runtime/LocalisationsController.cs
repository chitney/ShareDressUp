using SimpleJSON;
using System;
using UnityEngine;

public class LocalisationsController : Controller, ISerializable
{
    public static LocalisationsController Instance => Controllers.Get<LocalisationsController>();

    public Action OnLanguageChanged;

    private LocalisationDatabase LocalisationDatabase
    {
        get
        {
            if (_db == null) _db = DBManager.Get<LocalisationDatabase>();

            return _db;
        }
    }

    private LocalisationDatabase _db;


    public void SetLanguage(Language _language)
    {
        if (LocalisationDatabase.SetLanguage(_language))
        {
            OnLanguageChanged?.Invoke();
        }
    }

    public JSONObject Serialize()
    {
        JSONObject obj = new JSONObject();
        if (LocalisationDatabase != null)
            obj.Add("l", LocalisationDatabase.CurrentLanguage.name);
        return obj;
    }

    public void Deserialize(JSONObject obj)
    {
        Language language;
        if (obj!=null)
        {
            string l_key = obj["l"];
            language = (Language)LocalisationDatabase.Items.Find(l => l.name.Equals(l_key));
            if (language != null)
            {
                SetLanguage(language);
            }
        }
        else
        {
            SystemLanguage systemLanguage = Application.systemLanguage;
            language = (Language)LocalisationDatabase.Items.Find(l => l.name.Equals(systemLanguage.ToString()));
            if (language != null)
            {
                SetLanguage(language);
            }
            else
            {
                language = (Language)LocalisationDatabase.GetById(LocalisationDatabase.DefaultLanguageId);
                SetLanguage(language);
            }
        }
    }

    public string Name()
    {
        return "lclc";
    }


}
