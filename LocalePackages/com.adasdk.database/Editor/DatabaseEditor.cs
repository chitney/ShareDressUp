using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DBBase), true)]
public class DatabaseEditor<TItem> : Editor where TItem : DBItem
{
    protected DBBase myTarget;
    protected List<DBItem> myItems;

    private bool showScriptableItem = false;
    private bool editId = false;
    protected virtual void OnEnable()
    {
        myTarget = (DBBase)target;
        myItems = myTarget.Items;
        myItems.RemoveAll(i => i == null);
        showScriptableItem = (myItems.Count > 0 && myItems[0].hideFlags == HideFlags.None) ? true : false;
    }

    protected virtual List<DBItem> MyItems()
    {
        return new List<DBItem>(myTarget.Items);
    }

    public override void OnInspectorGUI()
    {
        DrawCreateBtn();
        DrawEditIDBtn();
        DrawScriptableBtn();
        DrawResetIdsBtn();

        DrawItem();
    }

    public virtual void DrawItem()
    {
        myItems = MyItems();

        foreach (var item in myItems)
        {
            DrawItem(item as DBItem);
        }
    }

    protected virtual void CreateItem<DBItem>()
    {
        myTarget.CreateNewInstance<TItem>();
    }

    protected virtual void RemoveItem(DBItem _item)
    {
        myTarget.Remove<DBItem>(_item);
    }

    protected virtual void DrawRemoveBtn(DBItem _item)
    {
        GUIEditorTools.DrawRemoveBtn(() =>
                RemoveItem(_item));
    }

    protected void DrawScriptableBtn()
    {
        if (GUILayout.Button((showScriptableItem ? "Hide" : "Show") + " Scriptable Item"))
        {
            showScriptableItem = !showScriptableItem;
            myTarget.ShowItemsObject(showScriptableItem);
        }
    }

    protected void DrawEditIDBtn()
    {
        if (GUILayout.Button(editId ? "LockID" : "EditId"))
        {
            editId = !editId;
        }
    }


    protected void DrawCreateBtn()
    {
        if (GUILayout.Button("Create"))
        {
            CreateItem<TItem>();
        }
    }

    protected void DrawResetIdsBtn()
    {
        if (GUILayout.Button("ResetIds"))
        {
            foreach (var item in myItems)
            {
                item.id = myItems.IndexOf(item);
            }
        }
    }

    protected virtual void DrawId(DBItem _item)
    {
        if (editId)
            _item.id = EditorGUILayout.IntField("ID", _item.id);
        else
            EditorGUILayout.LabelField("ID: " + _item.id);
    }

    protected virtual void DrawItem(DBItem _item)
    {
        if (_item != null)
        {
            GUI.enabled = false;
            DrawId(_item);
            GUI.enabled = true;
            EditorGUILayout.BeginHorizontal();
            _item.name = EditorGUILayout.TextField("Name:", _item.name);

            DrawRemoveBtn(_item);  

            EditorGUILayout.EndHorizontal();
        }

    }
}