using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TMPNumberAnimation))]
public class TMPNumberAnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var playOnAwake = serializedObject.FindProperty("_playOnAwake");
        var from = serializedObject.FindProperty("_from");
        var to = serializedObject.FindProperty("_to");
        var duration = serializedObject.FindProperty("_duration");
        var curve = serializedObject.FindProperty("_curve");

        var doFinalBounce = serializedObject.FindProperty("_doFinalBounce");
        var finalBounceScale = serializedObject.FindProperty("_finalBounceScale");
        var finalBounceDuration = serializedObject.FindProperty("_finalBounceDuration");

        var bumpScalePerDigit = serializedObject.FindProperty("_bumpScalePerDigit");
        var bumpDurationPerDigit = serializedObject.FindProperty("_bumpDurationPerDigit");
        var onFinish = serializedObject.FindProperty("_onFinishAnimation");

        EditorGUILayout.PropertyField(bumpScalePerDigit);
        EditorGUILayout.PropertyField(bumpDurationPerDigit);
        EditorGUILayout.PropertyField(onFinish);

        EditorGUILayout.Space();

        // PLAY ON AWAKE BLOCK
        EditorGUILayout.PropertyField(playOnAwake);

        if (playOnAwake.boolValue)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(from);
            EditorGUILayout.PropertyField(to);
            EditorGUILayout.PropertyField(duration);
            EditorGUILayout.PropertyField(curve);

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        // FINAL BOUNCE BLOCK
        EditorGUILayout.PropertyField(doFinalBounce);

        if (doFinalBounce.boolValue)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(finalBounceScale);
            EditorGUILayout.PropertyField(finalBounceDuration);

            EditorGUI.indentLevel--;
        }

        TMPNumberAnimation tmpNumberAnimation = (TMPNumberAnimation)target;
        
        if (GUILayout.Button("Play Default Animation"))
        {
            tmpNumberAnimation.PlayDefaultValues();
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}