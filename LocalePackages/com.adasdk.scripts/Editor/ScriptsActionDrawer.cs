using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ScriptAction))]
public class ScriptsActionDrawer : PropertyDrawer
{
    private ScriptAction me;

    GUIStyle style = new GUIStyle();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
        int index = 0;
        if (obj.GetType().IsGenericType)
        {
            index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
            me = ((List<ScriptAction>)obj)[index];
        }
       else if (obj.GetType().IsArray)
        {
            index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
            me = ((ScriptAction[])obj)[index];
        }
        else
            me = property.serializedObject.targetObject as ScriptAction;

        if (me!=null)
        {
            EditorGUI.LabelField(position, index + " "+ me.Description(), GUIEditorTools.colorGUIStyle(style, me.IsComplete ? Color.green : me.IsStarted? Color.yellow : Color.black));
        }
    }

}
