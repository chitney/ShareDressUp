using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioDatabase))]
internal class AudioDatabaseEditor : DatabaseEditor<AudioItem>
{
    protected override void DrawItem(DBItem _item)
    {
        if (_item != null)
        {
            AudioItem item = _item as AudioItem;
            if (item != null)
            {
                GUI.enabled = true;
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("ID: " + item.id);

                DrawRemoveBtn(item);

                EditorGUILayout.EndHorizontal();

                item.name = EditorGUILayout.TextField("Name:", item.name);
                item.Clip = (AudioClip)EditorGUI.ObjectField(EditorGUILayout.GetControlRect(false), item.Clip, typeof(AudioClip), false);
                item.defaultVolume = EditorGUILayout.FloatField("Volume:", item.defaultVolume);
                item.EnableReplay = EditorGUILayout.Toggle("EnableReplay ", item.EnableReplay);
                item.NeedToDublicate = EditorGUILayout.Toggle("NeedToDublicate ", item.NeedToDublicate);
                //GUIEditorTools.HorizontalLine(Color.gray);
            }
            else myTarget.Remove<DBItem>(_item);
        }
    }

}