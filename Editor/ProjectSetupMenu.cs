using UnityEngine;
using UnityEditor;
using System.IO;

namespace SetupTool
{
    public static class ProjectSetupMenu
    {
        private static SetupFoldersPreference preferenceData;

        //[MenuItem("Tools/Custom Tools/Project Setup/Create Project Folders")]
        //public static void CreateProjectFolders()
        //{
        //    CreateDirectories("_Project", "Models", "Materials", "Prefabs", "Scripts", "Scenes", "Shaders", "Textures");
        //    AssetDatabase.Refresh();
        //}
        //
        //[MenuItem("Tools/Custom Tools/Project Setup/Create Tool Folders")]
        //public static void CreateToolFolders()
        //{
        //    CreateDirectories("_Tools", "MyTool");
        //    AssetDatabase.Refresh();
        //}

        const string menuItem = "Tools/Custom Tools/Project Setup/";
        const string toolPath = "Packages/com.kiltec.setuptool/";

        [MenuItem(menuItem + "Folder Settings", priority = 31)]
        private static void ProjectViewSettings()
        {
            var path = $"{toolPath}Editor/Setup Folders.asset";

            if (!File.Exists(path)) { return; }

            var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            EditorGUIUtility.PingObject(asset);
            Selection.activeObject = asset;
        }

        [MenuItem(menuItem + "Setup Help", priority = 32)]
        private static void ProjectViewHelp()
        {
            var path = $"{toolPath}README.md";

            if (!File.Exists(path)) { return; }

            var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            EditorGUIUtility.PingObject(asset);
            Selection.activeObject = asset;
        }

        [MenuItem(menuItem + "Create Project Folders", priority = 10)]
        public static void CreateProjectFolders()
        {
            FindPreferences();
            CreateDirectories(preferenceData.projectFolder, preferenceData.subFolderNames);
            AssetDatabase.Refresh();
        }

        public static void CreateDirectories(string root, params string[] dir)
        {
            var fullPath = Path.Combine(Application.dataPath, root);
            foreach (var newDirectory in dir)
            {
                Directory.CreateDirectory(Path.Combine(fullPath, newDirectory));
            }
        }

        private static void FindPreferences()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(SetupFoldersPreference)}");
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                preferenceData = AssetDatabase.LoadAssetAtPath<SetupFoldersPreference>(path);
            }
        }
    }
}