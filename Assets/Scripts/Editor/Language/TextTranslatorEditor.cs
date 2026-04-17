using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextTranslator))]
public class TextTranslatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw default inspector first
        DrawDefaultInspector();

        GUILayout.Space(10);

        TextTranslator translator = (TextTranslator)target;

        if (GUILayout.Button("Update Translations"))
        {
            translator.UpdateTargetText();

            // Mark dirty so changes are saved in editor
            EditorUtility.SetDirty(translator);
        }
    }
}