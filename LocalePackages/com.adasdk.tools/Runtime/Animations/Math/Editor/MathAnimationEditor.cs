#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(MathAnimation), true)]
public class MathAnimationEditor : Editor
{
    private SerializedProperty PlayOnAwake;
    private SerializedProperty Loop;
    private SerializedProperty Disable;
    private SerializedProperty Speed;

    void OnEnable()
    {
        PlayOnAwake = serializedObject.FindProperty("PlayOnAwake");
        Loop = serializedObject.FindProperty("Loop");
        Disable = serializedObject.FindProperty("Disable");
        Speed = serializedObject.FindProperty("Speed");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        bool loop = Loop.boolValue;

        if (!loop)
        {
            EditorGUILayout.PropertyField(Disable);
        }
    }
}

#endif
