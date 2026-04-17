using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Toggle))]
public class ToggleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var toggleTypeProp = serializedObject.FindProperty("_toggleType");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_targetImage"));
        EditorGUILayout.PropertyField(toggleTypeProp);

        var type = (Toggle.Type)toggleTypeProp.enumValueIndex;

        if (type == Toggle.Type.ColorSwap)
        {
            DrawPropertiesExcluding(serializedObject,
                "_targetImage",
                "_toggleType",
                "_onSprite",
                "_offSprite"
            );
        }
        else if (type == Toggle.Type.SpriteSwap)
        {
            DrawPropertiesExcluding(serializedObject,
                "_targetImage",
                "_toggleType",
                "_onColor",
                "_offColor",
                "_duration"
            );
        }
        else
        {
            DrawPropertiesExcluding(serializedObject);
        }

        serializedObject.ApplyModifiedProperties();
    }
}