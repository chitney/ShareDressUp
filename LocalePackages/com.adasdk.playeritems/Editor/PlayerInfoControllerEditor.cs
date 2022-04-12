
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerInfoController))]
public class PlayerInfoControllerEditor : Editor
{
    PlayerInfoController myTarget;

    protected virtual void OnEnable()
    {
        myTarget = (PlayerInfoController)target;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("Current Level " + myTarget.Level.ToString());

        GUIEditorTools.HorizontalLine(Color.gray);

        foreach (Item item in myTarget.Items)
        {
            GUILayout.Label(item.Id + " " + item.Info.Name + " " + item.Count);
        }
    }

    public void ShowArrayProperty(SerializedProperty list)
    {
        EditorGUI.indentLevel += 1;
        for (int i = 0; i < list.arraySize; i++)
        {
            int id = list.GetArrayElementAtIndex(i).FindPropertyRelative("id").intValue;

            var itemInfo = ItemsDatabase.Instance.GetItemById(id);

            string itemName = itemInfo.name;

            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new UnityEngine.GUIContent(itemName), true);
        }
        EditorGUI.indentLevel -= 1;
    }


}
