using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(DBItemAttribute))]
public class DBItemAttributeDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DBItemAttribute idAttribute = attribute as DBItemAttribute;

        if (!idAttribute.type.IsSubclassOf(typeof(DBBase)))
        {
            EditorGUI.LabelField(position, label.text, "Not a child of DBBase ");
            return;
        }

        DBBase db = DBBase.GetInstanceByType(idAttribute.type);
        if (db != null)
        {
            int selectedIndex = db.Items.FindIndex(e => e.id == property.intValue) + 1;
            selectedIndex = EditorGUI.Popup(position: position, label: property.name, selectedIndex: selectedIndex, displayedOptions: db.GetEntriesList());
            property.intValue = selectedIndex < 1 || selectedIndex > db.Items.Count ? -1 : db.Items[selectedIndex - 1].id;
        }
        else
            EditorGUI.LabelField(position, "There is no " + idAttribute.type);
    }

}