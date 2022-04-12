using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(SriptDBItem))]
public class SriptDBItemEditor : Editor
{
    private SriptDBItem hsTarget;

    public override VisualElement CreateInspectorGUI()
    {
        hsTarget = (SriptDBItem)target;
        return base.CreateInspectorGUI();
    }

    Editor _hashEditor;

    private GUIStyle guiStyle = new GUIStyle();

    public override void OnInspectorGUI()
    {
        if (hsTarget == null) return;
        if (hsTarget.PlayerScriptPrefab == null) return;

        hsTarget.name = EditorGUILayout.TextField("Name", hsTarget.name);
        if (hsTarget.PlayerScriptPrefab.gameObject.name != hsTarget.name)
        {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(hsTarget.PlayerScriptPrefab.gameObject), hsTarget.name);
        }
        hsTarget.Level = EditorGUILayout.IntField("Level", hsTarget.Level);

        EditorGUILayout.ObjectField(hsTarget.PlayerScriptPrefab, typeof(PlayerScript),true);

        GUIEditorTools.HorizontalLine(Color.gray);

        #region Actions List

        EditorGUILayout.LabelField("Actions", GUIEditorTools.LabelGUIStyle(guiStyle, Color.black,FontStyle.Bold));
        
        if (hsTarget.PlayerScriptPrefab.Actions == null)
        {
            hsTarget.PlayerScriptPrefab.InitActions();
        }
        
        var list = new List<ScriptAction>(hsTarget.PlayerScriptPrefab.Actions);

        foreach (ScriptAction action in list)
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical(GUILayout.Width(25f));
            EditorGUILayout.LabelField(list.IndexOf(action).ToString(), GUILayout.Width(25f));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            _hashEditor = CreateEditor(action);
            _hashEditor.serializedObject.Update();
            _hashEditor.OnInspectorGUI();
            _hashEditor.serializedObject.ApplyModifiedProperties();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(50f));
            if (GUILayout.Button("▲"))
            {
                MoveActionUp(action);
            }
            if (GUILayout.Button("▼"))
            {
                MoveActionDown(action);
            }

            GUIEditorTools.DrawRemoveBtn(()=>
                DeleteAction(action));

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(5f);
        }
        

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Add"))
        {
            ShowActionsList();
        }
        GUI.backgroundColor = Color.white;
        #endregion Actions List
    }

    private void MoveActionUp(ScriptAction action)
    {
        hsTarget.PlayerScriptPrefab.MoveActionUp(action);
    }

    private void MoveActionDown(ScriptAction action)
    {
        hsTarget.PlayerScriptPrefab.MoveActionDown(action);
    }

    private void DeleteAction(ScriptAction action)
    {
        hsTarget.PlayerScriptPrefab.DeleteAction(action);
    }

    private void ShowActionsList()
    {
        void AddAction(Type _type)
        {
            hsTarget.PlayerScriptPrefab.AddAction(_type);
        }

        var list = GUIEditorTools.GetTypes(typeof(ScriptAction));

        var menu = new GenericMenu();

        foreach (Type type in list)
        {
            string name = type.GetField("Name").GetValue(null).ToString();
            menu.AddItem(new GUIContent(name), false, () => AddAction(type));
        }
        menu.ShowAsContext();
    }


}
