using System.IO;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ScriptsDatabase))]
public class ScriptsDatabaseEditor : DatabaseEditor<SriptDBItem>
{
    private static readonly string ItemsFolder = "Assets/DB/Scripts/";

    public static PlayerScript CreateScriptsPrefab()
    {
        if (!Directory.Exists(ItemsFolder))
            Directory.CreateDirectory(ItemsFolder);

        GameObject obj = new GameObject();
        obj.AddComponent<PlayerScript>();
        string prefabPath = ItemsFolder + obj.name + ".prefab";
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(obj.gameObject, prefabPath);
        DestroyImmediate(obj);
        return prefab.GetComponent<PlayerScript>();
    }

    protected override void RemoveItem(DBItem _item)
    {
        SriptDBItem item = _item as SriptDBItem;
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item.PlayerScriptPrefab.gameObject));
        base.RemoveItem(_item);
    }

    protected override void CreateItem<DBItem>()
    {
        SriptDBItem item = myTarget.CreateNewInstance<SriptDBItem>();
        item.PlayerScriptPrefab = CreateScriptsPrefab();
        item.PlayerScriptPrefab.Info = item;
    }

}
