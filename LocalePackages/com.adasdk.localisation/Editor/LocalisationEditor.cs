using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalisationDatabase))]
internal class LocalisationEditor : DatabaseEditor<Language>
{
    LocalisationDatabase localeDB;
    protected override void OnEnable()
    {
        base.OnEnable();
        localeDB = (LocalisationDatabase)target;
    }

    string findTextFieldText = "";
    public TextAsset TextLocale;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Current Language", localeDB.CurrentLanguage == null ? "null" : localeDB.CurrentLanguage.Name);

        TextLocale = (TextAsset)EditorGUILayout.ObjectField(TextLocale, typeof(TextAsset), false);

        EditorGUILayout.IntField("Default Language Id", localeDB.DefaultLanguageId);

        if (TextLocale != null)
        {
            if (GUILayout.Button("Load .txt file"))
            {
                localeDB.LoadData(TextLocale);
            }
        }
        //else
            //EditorGUILayout.LabelField("TextLocale is null", GUIEditorTools.colorGUIStyle(Color.red));


        findTextFieldText = EditorGUILayout.TextField("Find by key", findTextFieldText);
        if (!findTextFieldText.Equals("") && localeDB.CurrentLanguage != null)
        {
            var finded = localeDB.FindByKey(findTextFieldText);
            foreach (var l in finded)
                EditorGUILayout.LabelField("[" + l.key + "]" + " " + l.translate);
        }
    }

}
