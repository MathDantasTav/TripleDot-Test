using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageManager))]
public class LanguageManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LanguageManager manager = (LanguageManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Update cached translations"))
        {
            manager.UpdateCachedTranslations();
        }
    }
}