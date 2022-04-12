using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class DBBase : ScriptableObject
{
    public List<DBItem> Items = new List<DBItem>();
    public static DBBase GetInstanceByType(Type type)
    {
        MethodInfo method = typeof(DBBase).GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static);
        method = method.MakeGenericMethod(type);
        DBBase methodReturn = method.Invoke(null, null) as DBBase;
        return methodReturn;
    }

    public static T GetInstance<T>() where T : DBBase
    {
        Type type = typeof(T);
        PropertyInfo property = type.GetProperty("Instance");
        return property != null ? property.GetValue(property, null) as T : null;
    }

    public string[] GetEntriesList()
    {
        string[] textList = new string[Items.Count + 1];
        textList[0] = "None";
        for (int i = 0; i < Items.Count; i++)
        {
            DBItem ent = Items[i];
            textList[i + 1] = string.Format("{0} (id: {1})", ent.name, ent.id);
        }
        return textList;
    }

    public virtual void Refresh() { }

    public DBItem GetById(int id)
    {
        return Items.Find(i => i.id == id);
    }

#if UNITY_EDITOR

    private bool CheckId(int id)
    {
        return Items.FindAll(i => i.id == id).Count<=1;
    }

    private void ReimportAsset()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
    }

    public virtual TItem CreateNewInstance<TItem>() where TItem : DBItem
    {
        var ent = CreateInstance<TItem>();
        Items.Add(ent);
         
        int id = Items.IndexOf(ent);

        if (CheckId(id))
            ent.id = id;

        else
        {
            Debug.LogError("double ID");
            id++;
            while (!CheckId(id) && id < 2000)
            {
                id++;
            }
            ent.id = id;
        }
           

        ent.owner = this;
        AssetDatabase.AddObjectToAsset(ent, this);
        ent.hideFlags = HideFlags.None;
        ent.name = "new";
        ReimportAsset();
        Refresh();
        return ent;
    }

    public void Remove<TItem>(DBItem item) where TItem : DBItem
    {
        Items.Remove(item as TItem);
        DestroyImmediate(item, true);
        ReimportAsset();
        Refresh();
    }

    public void RemoveAll()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            DBItem item = Items[i];
            DestroyImmediate(item, true);
        }
        Items.Clear();
        Refresh();
        ReimportAsset();
    }


    public virtual void ShowItemsObject(bool _show)
    {
        foreach (var item in Items)
        {
            item.hideFlags = _show ? HideFlags.None : HideFlags.HideInHierarchy;
        }
        ReimportAsset();
    }
#endif
}
