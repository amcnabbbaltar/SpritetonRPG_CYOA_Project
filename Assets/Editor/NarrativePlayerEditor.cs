#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueSystem.NarrativePlayer))]
public class NarrativePlayerEditor : Editor
{
    // Cached property refs
    SerializedProperty modeProp;
    SerializedProperty inkJSONAssetProp;
    SerializedProperty localFolderProp;
    SerializedProperty localFileNameProp;

    void OnEnable()
    {
        modeProp          = serializedObject.FindProperty("mode");
        inkJSONAssetProp  = serializedObject.FindProperty("inkJSONAsset");
        localFolderProp   = serializedObject.FindProperty("localFolder");
        localFileNameProp = serializedObject.FindProperty("localFileName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw Mode first
        EditorGUILayout.PropertyField(modeProp);

        var selectedMode = (DialogueSystem.NarrativePlayer.NarrativeMode)modeProp.enumValueIndex;
        EditorGUILayout.Space(6);

        // Conditional block
        switch (selectedMode)
        {
            case DialogueSystem.NarrativePlayer.NarrativeMode.Ink:
                EditorGUILayout.LabelField("Ink Source (compiled JSON)", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(inkJSONAssetProp);
                break;

            case DialogueSystem.NarrativePlayer.NarrativeMode.Twee:
                EditorGUILayout.LabelField("Twee Source (local text file)", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(localFolderProp);
                EditorGUILayout.PropertyField(localFileNameProp);
                break;

            case DialogueSystem.NarrativePlayer.NarrativeMode.Auto:
                EditorGUILayout.HelpBox(
                    "Auto mode: If an Ink JSON is assigned, Ink will run; otherwise Twee will run.",
                    MessageType.Info
                );
                EditorGUILayout.LabelField("Ink (optional)", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(inkJSONAssetProp);

                EditorGUILayout.LabelField("Twee (fallback if no Ink)", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(localFolderProp);
                EditorGUILayout.PropertyField(localFileNameProp);
                break;
        }

        EditorGUILayout.Space(10);

        // Draw the rest of the component fields except the ones we handled above.
        // Add any additional fields you want to hide to this exclusion list.
        DrawPropertiesExcluding(
            serializedObject,
            "m_Script",     // default Unity script reference
            "mode",
            "inkJSONAsset",
            "localFolder",
            "localFileName",
            "tweeParser",   // runtime-only
            "passages",     // runtime-only
            "currentPassageTitle", // runtime-only
            "story",        // runtime-only
            "myChoices"     // runtime-only
        );

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
