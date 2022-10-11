using UnityEngine;
using UnityEditor;
using System.IO;

namespace SetupTool
{
    public static class ProjectSetupMenu
    {
        private static SetupFoldersPreference preferenceData;

        const string menuItem = "Tools/Custom Tools/Project Setup/";
        const string toolPath = "Packages/com.kiltec.setuptool/";

        [MenuItem(menuItem + "Folder Settings", priority = 31)]
        private static void ProjectViewSettings()
        {
            var path = $"{toolPath}Editor/Default Setup Folders.asset";

            if (!File.Exists(path)) { return; }

            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            EditorGUIUtility.PingObject(asset);
            Selection.activeObject = asset;
        }

        [MenuItem(menuItem + "Setup Help", priority = 32)]
        private static void ProjectViewHelp()
        {
            var path = $"{toolPath}README.md";

            if (!File.Exists(path)) { return; }

            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            EditorGUIUtility.PingObject(asset);
            Selection.activeObject = asset;
        }

        [MenuItem(menuItem + "Import Script Templates", priority = 20)]
        public static void ImportScriptTemplates()
        {
            var dirPath = Path.Combine(Application.dataPath, "ScriptTemplates");
            Directory.CreateDirectory(dirPath);
            CopyFiles(dirPath);
        }

        [MenuItem(menuItem + "Create Project Folders", priority = 10)]
        public static void CreateProjectFolders()
        {
            FindPreferences();
            CreateDirectories(preferenceData.projectFolder, preferenceData.subFolders);
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

        private static void CopyFiles(string destinationPath)
        {
            var dataPath = Application.dataPath.Replace("Assets", @"Packages\com.kiltec.setuptool");
            string[] filePaths = Directory.GetFiles($@"{dataPath}\ScriptTemplates\");

            foreach (string file in filePaths)
            {
                FileInfo info = new FileInfo(file);
                string sourceFile = info.FullName;
                string destinationFile = $@"{destinationPath}\{info.Name}";

                //Debug.Log(sourceFile);
                //Debug.Log(destinationFile);

                if (!File.Exists(destinationFile) && !destinationFile.Contains("meta"))
                {
                    File.Copy(sourceFile, destinationFile);
                }
            }

            Debug.Log("Unity must be restarted to use the new script templates!");
            AssetDatabase.Refresh();
        }
    }
}