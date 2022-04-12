using System;
using System.Collections.Generic;

[Serializable]
public struct LocalizedString
{
    public string key;
    public string translate;

    public LocalizedString(string key, string translate)
    {
        this.key = key;
        this.translate = translate;
    }
}

public class Language : DBItem
{
    public string Name => name;

    public string Key = "";

    public bool IsCurrent
    {
        get
        {
            return LocalisationDatabase.Instance.CurrentLanguage.id == id;
        }
    }

    public List<LocalizedString> LocalizedStrings;

    public string GetByKey(string key)
    {
        var find = LocalizedStrings.Find(l => l.key == key);
        return find.translate == null ? "" : find.translate;
    }

#if UNITY_EDITOR
    public List<LocalizedString> FindByKey(string key)
    {
        return LocalizedStrings.FindAll(l => l.key.Contains(key));
    }

#endif
}
