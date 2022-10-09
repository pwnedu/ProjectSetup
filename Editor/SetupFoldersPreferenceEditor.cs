using UnityEngine;
using UnityEditor;

namespace SetupTool
{
    [CustomEditor(typeof(SetupFoldersPreference))]
    public class SetupFoldersPreferenceEditor : Editor
    {
        // Controller
        private SerializedObject styleController;

        // Extensions
        private SerializedProperty projectFolder;
        private SerializedProperty subFolders;

        // Editor Properties
        GUIStyle headerStyle;

        private Color headingColor = new Color32(225, 205, 140, 255);
        private Color subHeadingColor = new Color32(225, 155, 155, 255);

        private void OnEnable()
        {
            // Initialise Controller
            styleController = new SerializedObject(target);

            // Find Property
            projectFolder = styleController.FindProperty("projectFolder");
            subFolders = styleController.FindProperty("subFolderNames");

            // Creaete Header GUI Style
            SetHeaderStyle();
        }

        private void SetHeaderStyle()
        {
            headerStyle = new GUIStyle()
            {
                normal = new GUIStyleState()
                {
                    textColor = headingColor
                },
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.LowerCenter,
                fixedHeight = 30f,
                richText = true,
                fontSize = 18,
            };
        }

        public override void OnInspectorGUI()
        {
            var previousColour = GUI.contentColor;

            // Scriptable Title
            GUILayout.Label("Custom Folder Setup", headerStyle);
            GUILayout.Label(target.name, EditorStyles.centeredGreyMiniLabel);
            GUILayout.Space(10);

            EditorGUI.BeginChangeCheck();

            // Area Heading
            GUI.contentColor = subHeadingColor;
            EditorGUILayout.LabelField($"My Project Preferences", EditorStyles.largeLabel);
            GUI.contentColor = previousColour;
            GUILayout.Space(5);

            // Custom Notes Styling
            EditorGUILayout.PropertyField(projectFolder);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(subFolders);

            if (EditorGUI.EndChangeCheck())
            {
                styleController.ApplyModifiedProperties();
            }
        }
    }
}