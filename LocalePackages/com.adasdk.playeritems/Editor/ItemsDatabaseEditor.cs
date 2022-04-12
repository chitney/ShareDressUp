using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemsDatabase))]
public class ItemsDatabaseEditor : DatabaseEditor<ItemsInfo>
{
    protected override void DrawItem(DBItem _item)
    {
        if (_item != null)
        {
            ItemsInfo item = _item as ItemsInfo;
            if (item != null)
            {
                GUI.enabled = true;
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("ID: " + item.id);

                DrawRemoveBtn(item);

                EditorGUILayout.EndHorizontal();
                item.name = EditorGUILayout.TextField("Name:", item.name);
                item.DefaultCount = EditorGUILayout.IntField("Default count:", item.DefaultCount);
                item.image = (Sprite)EditorGUILayout.ObjectField(item.image, typeof(Sprite), false, GUILayout.Width(50), GUILayout.Height(50));
                GUIEditorTools.HorizontalLine(Color.gray);
            }
            else myTarget.Remove<DBItem>(_item);
        }
    }
}
