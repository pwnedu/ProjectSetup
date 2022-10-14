using UnityEngine;
using UnityEditor;

namespace SetupTool
{
    [CustomEditor(typeof(SetupFoldersPreference))]
    public class SetupFoldersPreferenceEditor : Editor
    {
        // Controller
        private SetupFoldersPreference setupFolders;
        private SerializedObject styleController;

        // Extensions
        private SerializedProperty projectFolder;
        private SerializedProperty subFolders;
        private SerializedProperty specialFolders;
        private SerializedProperty structures;

        // Editor Properties
        GUIStyle headerStyle;
        GUIStyle folderGUIStyle;
        GUIStyle slashGUIStyle;
        GUIStyle labelGUIStyle;
        GUIStyle buttonGUIStyle;

        GUIContent pathContent;

        private Color headingColor = new Color32(225, 205, 140, 255);
        private Color subHeadingColor = new Color32(225, 155, 155, 255);

        int selected = 0;

        private void OnEnable()
        {
            // Initialise Controller
            styleController = new SerializedObject(target);
            setupFolders = target as SetupFoldersPreference;

            // Find Property
            projectFolder = styleController.FindProperty("projectFolder");
            subFolders = styleController.FindProperty("subFolders");
            specialFolders = styleController.FindProperty("specialFolders");
            structures = styleController.FindProperty("structures");

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

            folderGUIStyle = new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = GetTextColour() },
                alignment = TextAnchor.MiddleRight,
                fontStyle = FontStyle.Normal,
                fontSize = 12,
                richText = true,
            };

            slashGUIStyle = new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = GetTextColour() },
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Normal,
                fontSize = 12,
                richText = true,
            };

            labelGUIStyle = new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = GetTextColour() },
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                richText = true,
            };

            buttonGUIStyle = new GUIStyle(EditorStyles.miniButton)
            {
                normal = new GUIStyleState() { textColor = GetTextColour() },
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                richText = true,
            };

            pathContent = new GUIContent()
            {
                text = "Root Folder: ",
                tooltip = "The path to setup your folders."
            };
        }

        public override void OnInspectorGUI()
        {
            var previousColour = GUI.contentColor;
            var height = 10;

            // Scriptable Title
            GUILayout.Label("Custom Folder Setup", headerStyle);
            GUILayout.Label(target.name, EditorStyles.centeredGreyMiniLabel);
            GUILayout.Space(height);

            EditorGUI.BeginChangeCheck();

            // Area Heading
            GUI.contentColor = subHeadingColor;
            EditorGUILayout.LabelField($"My Project Folders", EditorStyles.largeLabel);
            GUI.contentColor = previousColour;
            GUILayout.Space(height);

            // Folder Setup
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(pathContent, labelGUIStyle, GUILayout.Width(80));
            EditorGUILayout.LabelField("Assets/", folderGUIStyle, GUILayout.Width(60));
            EditorGUILayout.PropertyField(projectFolder, GUIContent.none);
            EditorGUILayout.LabelField("/", slashGUIStyle, GUILayout.Width(20));
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            EditorGUILayout.PropertyField(subFolders);
            EditorGUILayout.BeginHorizontal();
            GUIContent project = new GUIContent() { text = "Create Project Folders", tooltip = "Create the above folder structure." };
            if (GUILayout.Button(project, buttonGUIStyle))
            {
                ProjectSetupMenu.CreateDirectories(setupFolders.projectFolder, setupFolders.subFolders);
                PingFolder(setupFolders.projectFolder);
            }
            GUIContent every = new GUIContent() { text = "Create All Folders", tooltip = "Create all folders listed for setup at once." };
            if (GUILayout.Button(every, buttonGUIStyle))
            {
                ProjectSetupMenu.CreateDirectories(setupFolders.projectFolder, setupFolders.subFolders);
                ProjectSetupMenu.CreateSpecialFolders();
                CreateAllSubFolders();
                PingFolder(setupFolders.structures[selected].assetFolderPath);
            }
            EditorGUILayout.EndHorizontal();

            var maxLength = setupFolders.structures.Length - 1;
            var width = 18;

            // Area Heading
            GUILayout.Space(height);
            GUI.contentColor = subHeadingColor;
            EditorGUILayout.LabelField($"My Sub Structures", EditorStyles.largeLabel);
            GUI.contentColor = previousColour;
            GUILayout.Space(height);

            // Custom Structures
            EditorGUILayout.PropertyField(structures);
            GUILayout.Space(height);

            if (setupFolders.structures.Length > 0)
            {
                if (selected >= setupFolders.structures.Length) { selected -= 1; }
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Selection: " + setupFolders.structures[selected].assetFolderPath, labelGUIStyle, GUILayout.MinWidth(90));
                if (GUILayout.Button("<", buttonGUIStyle, GUILayout.Width(width)))
                {
                    if (selected == 0) { selected = maxLength; }
                    else { selected -= 1; }
                }
                selected = EditorGUILayout.IntField(selected, GUILayout.Width(width));
                if (GUILayout.Button(">", buttonGUIStyle, GUILayout.Width(width)))
                {
                    if (selected == maxLength) { selected = 0; }
                    else { selected += 1; }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUIContent select = new GUIContent() { text = "Create Selected", tooltip = "Create the selected folder structure." };
                if (GUILayout.Button(select, buttonGUIStyle))
                {
                    ProjectSetupMenu.CreateDirectories(setupFolders.structures[selected].assetFolderPath, setupFolders.structures[selected].subFolders);
                    PingFolder(setupFolders.structures[selected].assetFolderPath);
                }
                GUIContent multi = new GUIContent() { text = "Create Multiple", tooltip = "Create all the sub folder structures at once." };
                if (GUILayout.Button(multi, buttonGUIStyle))
                {
                    CreateAllSubFolders();
                    PingFolder(setupFolders.structures[selected].assetFolderPath);
                }
                EditorGUILayout.EndHorizontal();
            }
            
            // Area Heading
            GUILayout.Space(height);
            GUI.contentColor = subHeadingColor;
            EditorGUILayout.LabelField($"Unity Special Folders", EditorStyles.largeLabel);
            GUI.contentColor = previousColour;
            GUILayout.Space(height);

            // Custom Structures
            EditorGUILayout.PropertyField(specialFolders);

            GUIContent special = new GUIContent() { text = "Create Unity Special Folders", tooltip = "Create all the Unity special folders." };
            if (GUILayout.Button(special, buttonGUIStyle))
            {
                ProjectSetupMenu.CreateSpecialFolders();
                PingFolder();
            }

            if (EditorGUI.EndChangeCheck())
            {
                styleController.ApplyModifiedProperties();
            }
        }

        private Color GetTextColour() // temporary switch to GUIExtensions
        {
            Color darkModeTextColour = new Color(0.75f, 0.75f, 0.75f);
            Color normalModeTextColour = new Color(0.25f, 0.25f, 0.25f);

            return EditorGUIUtility.isProSkin ? darkModeTextColour : normalModeTextColour;
        }

        private void CreateAllSubFolders()
        {
            for (int i = 0; i < setupFolders.structures.Length; i++)
            {
                ProjectSetupMenu.CreateDirectories(setupFolders.structures[i].assetFolderPath, setupFolders.structures[i].subFolders);
            }
        }

        private void PingFolder()
        {
            AssetDatabase.Refresh();

            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets");
            EditorGUIUtility.PingObject(asset);
        }

        private void PingFolder(string folder)
        {
            AssetDatabase.Refresh();

            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/" + folder);
            EditorGUIUtility.PingObject(asset);
        }
    }
}