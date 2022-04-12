using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Localisation", menuName = "ScriptableObjects/DB/Localisation")]
public class LocalisationDatabase : DBBase
{
    public static LocalisationDatabase Instance { get { return DBManager.Get<LocalisationDatabase>(); } }
    public Language CurrentLanguage { get => currentLanguage; }

    public int DefaultLanguageId;

    [SerializeField]
    private Language currentLanguage;

    public string GetByKey(string _key)
    {
        if (CurrentLanguage == null) return "";
        return CurrentLanguage.GetByKey(_key);
    }

    public bool SetLanguage(Language language)
    {
        if (CurrentLanguage != language)
        {
            currentLanguage = language;
            return true;
        }
        return false;
    }

    #region EDITOR
#if UNITY_EDITOR
    public List<LocalizedString> FindByKey(string _key)
    {
        return CurrentLanguage.FindByKey(_key);
    }
    public static List<List<string>> Parse(TextAsset ta)
    {
        List<List<string>> table = new List<List<string>>();
        List<string> tempLine = new List<string>();
        string[] lines = ta.text.Split(new char[] { '\r', '\n' });
        foreach (string line in lines)
        {
            if (!line.Equals(""))
            {
                tempLine.Clear();
                string[] _l = line.Split('\t');
                foreach (string row in _l)
                {
                    row.Replace(" ", "");
                    tempLine.Add(row);
                }

                table.Add(new List<string>(tempLine));
            }

        }
        return table;
    }
    public void LoadData(TextAsset textLocale)
    {
        RemoveAll();

        var table = Parse(textLocale);
        int lineCount = table.Count;
        int rowCount = table[0].Count;

        Language[] languages = new Language[rowCount];

        for (int row = 0; row < rowCount; row++)
        {
            if (table[0][row] != "key")
            {
                Language l = CreateNewInstance<Language>();
                l.name = table[0][row];
                languages[row] = l;
            }
        }

        for (int row = 1; row < rowCount; row++)
        {
            var temp = new List<LocalizedString>();
            for (int line = 1; line < lineCount; line++)
            {
                temp.Add(new LocalizedString(table[line][0], table[line][row]));
                languages[row].LocalizedStrings = temp;
            }
        }
    }


#endif
    #endregion EDITOR
}
