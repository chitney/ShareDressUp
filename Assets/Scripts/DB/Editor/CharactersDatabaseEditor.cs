
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(CharactersDatabase))]
public class CharactersDatabaseEditor : DatabaseEditor<CharactersInfo>
{
    /// <summary>
    /// !!! without Assets/
    /// </summary>
    private const string PsbsFolder = "Art/DressUp/characters";

    private const string PsbsPath = "Assets/Art/DressUp/characters";

    private static List<T> GetAtPath<T>(string path)
    {
        ArrayList al = new ArrayList();
        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

        foreach (string fileName in fileEntries)
        {
            string temp = fileName.Replace("\\", "/");
            int index = temp.LastIndexOf("/");
            string localPath = "Assets/" + path;

            if (index > 0)
                localPath += temp.Substring(index);

            Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

            if (t != null)
                al.Add(t);
        }

        List<T> result = new List<T>();

        for (int i = 0; i < al.Count; i++)
            result.Add((T)al[i]);

        return result;
    }

    protected void DrawUptadeDBBtn()
    {
        if (GUILayout.Button("UpdateDB"))
        {
            UpdateDB();
        }
    }

    public override void OnInspectorGUI()
    {
        DrawCreateBtn();
        DrawEditIDBtn();
        DrawScriptableBtn();
        DrawResetIdsBtn();
        DrawUptadeDBBtn();

        var serializedObject = new SerializedObject(target);
        var property = serializedObject.FindProperty("Bachgrounds");
        serializedObject.Update();
        EditorGUILayout.PropertyField(property, true);
        serializedObject.ApplyModifiedProperties();

        DrawItem();
    }

    private void UpdateDB()
    {
        List<Object> assets = GetAtPath<Object>(PsbsFolder);
        CharactersDatabase db = CharactersDatabase.Instance;

        foreach (Object obj in assets)
        {
            if (obj is GameObject)
            {
                GameObject psb = obj as GameObject;

                CharactersInfo characterInfo = db.GetItemByName(psb.name);
                if (characterInfo == null)
                {
                    //Create a character
                    characterInfo = db.CreateNewItem();
                }

                characterInfo.name = psb.name;

                //load icon

                Object[] icon_texture = AssetDatabase.LoadAllAssetsAtPath(PsbsPath + "/" + psb.name + "_icon.png");

                foreach (Object tex in icon_texture)
                    if (tex is Sprite)
                        characterInfo.Icon = tex as Sprite;

                //load the characters items
                List<Transform> children = new List<Transform>();
                psb.transform.GetComponentsInChildren(children);

                var groups = children.FindAll(o => o.name.Contains("#"));

                characterInfo.Items = new CharachtersItem[groups.Count];
                CharachtersItem currentItem;
                Transform currentGroup;
                for (int i = 0; i < groups.Count; i++)
                {
                    characterInfo.Items[i] = new CharachtersItem();

                    currentItem = characterInfo.Items[i];
                    currentGroup = groups[i];

                    currentItem.category = (Category)System.Enum.Parse(typeof(Category), currentGroup.name.Split('#')[1]);
                    currentItem.isDefault = currentGroup.name.Contains("?");

                    switch (currentItem.category)
                    {
                        case Category.body:
                            currentItem.SiblingPosition = SiblingPosition.First;
                            break;
                        case Category.eyes:
                            currentItem.SiblingPosition = SiblingPosition.Second;
                            break;
                        case Category.lips:
                            currentItem.SiblingPosition = SiblingPosition.Second;
                            break;
                        case Category.brows:
                            currentItem.SiblingPosition = SiblingPosition.Second;
                            break;
                        case Category.hairs:
                        case Category.Beards:
                            currentItem.SiblingPosition = SiblingPosition.Last;
                            break;
                        default:
                            currentItem.SiblingPosition = SiblingPosition.BeforeLast;
                            break;
                    }

                    var variants = new List<SpriteRenderer>();
                    currentGroup.GetComponentsInChildren(variants);

                    currentItem.name = currentGroup.name;
                    currentItem.Icon = variants[0].sprite;

                    int cnt = variants.Count;
                    currentItem.Variants = new Variant[cnt];

                    for (int j = 0; j < variants.Count; j++)
                    {
                        SpriteRenderer icon = variants[j];
                        Sprite sprite = null;

                        Object[] texture = AssetDatabase.LoadAllAssetsAtPath(PsbsPath + "/"+ psb.name +"/"+ icon.name + ".png");
                        foreach (Object tex in texture)
                            if (tex is Sprite)
                                sprite = tex as Sprite;
                            

                        currentItem.Variants[j] = new Variant(icon.sprite, sprite);
                    }

                }
            
            }
        }
    }
}
