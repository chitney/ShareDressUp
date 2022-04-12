using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class GUIEditorTools 
{
    static List<Type> allTypes = new List<Type>();
    static List<Type> selectedTypes = new List<Type>();
    public static List<Type> GetTypes(Type _type)
    {
        allTypes.Clear();
        selectedTypes.Clear();

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        if (assemblies != null)
        {
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                    allTypes.AddRange(assembly.GetTypes());
            }
        }

        allTypes.RemoveAll(t => t == null);

        foreach (Type type in allTypes)
        {
            if (_type.IsAssignableFrom(type) && !type.ContainsGenericParameters && !type.IsAbstract)
                selectedTypes.Add(type);
        }

        return selectedTypes;
        
    }

    public static GUIStyle colorGUIStyle(GUIStyle s,  Color _color)
    {
        s.normal.textColor = _color;
        return s;
    }

    public static GUIStyle LabelGUIStyle(GUIStyle s, Color _color, FontStyle style = FontStyle.Normal, int size = 12)
    {
        s.fontStyle = style;
        s.fontSize = size;
        return colorGUIStyle(s, _color);
    }

    public static void HorizontalLine(Color color)
    {
        GUIStyle horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset(0, 0, 4, 4);
        horizontalLine.fixedHeight = 1;

        var c = GUI.color;
        GUI.color = color;
        GUILayout.Box(GUIContent.none, horizontalLine);
        GUI.color = c;
    }

    public static void DrawRemoveBtn(Action onClick)
    {
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("X", GUILayout.Width(50)))
        {
            onClick?.Invoke();
        }
        GUI.backgroundColor = Color.white;
    }
}
