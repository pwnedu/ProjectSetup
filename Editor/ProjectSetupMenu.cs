using UnityEngine.Rendering;
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

        #region Folders Menu Items

        [MenuItem(menuItem + "Create Folders/Special Folders", priority = 11)]
        public static void CreateSpecialFolders()
        {
            FindPreferences();
            CreateDirectories("", preferenceData.specialFolders);
            AssetDatabase.Refresh();
        }

        [MenuItem(menuItem + "Create Folders/Project Folders", priority = 10)]
        public static void CreateProjectFolders()
        {
            FindPreferences();
            CreateDirectories(preferenceData.projectFolder, preferenceData.subFolders);
            AssetDatabase.Refresh();
        }

        #endregion

        #region Templates Menu Items

        [MenuItem(menuItem + "Import Templates/Script Templates", priority = 20)]
        public static void ImportScriptTemplates()
        {
            CopyFiles("Editor/Templates/ScriptTemplates", DestinationPath());
        }

        [MenuItem(menuItem + "Import Templates/Editor Templates", priority = 21)]
        public static void ImportEditorTemplates()
        {
            CopyFiles("Editor/Templates/EditorTemplates", DestinationPath());
        }

        [MenuItem(menuItem + "Import Templates/Shader Templates", priority = 22)]
        public static void ImportShaderTemplates()
        {
            // Get Render Pipeline
            var pipeline = GraphicsSettings.currentRenderPipeline;
            UnityRenderer renderer;

            if (pipeline == null) 
            { 
                renderer = UnityRenderer.SRP;
                CopyFiles($"Editor/Templates/ShaderTemplates/SRP", DestinationPath());
            }
            else if (pipeline.ToString().Contains("Universal"))
            { 
                renderer = UnityRenderer.URP;
                CopyFiles($"Editor/Templates/ShaderTemplates/URP", DestinationPath());
            }
            else if (pipeline.ToString().Contains("Lightweight"))
            {
                renderer = UnityRenderer.LWRP;
                CopyFiles($"Editor/Templates/ShaderTemplates/URP", DestinationPath());
            }
            else 
            { 
                renderer = UnityRenderer.HDRP;
                CopyFiles($"Editor/Templates/ShaderTemplates/HDRP", DestinationPath());
            }

            Debug.Log($"Shader Templates imported for current Render Pipeline: {renderer}");
        }

        [MenuItem(menuItem + "Import Templates/File Templates", priority = 23)]
        public static void ImportFileTemplates()
        {
            CopyFiles("Editor/Templates/FileTemplates", DestinationPath());
        }

        [MenuItem(menuItem + "Import Templates/All Templates", priority = 60)]
        public static void ImportAllTemplates()
        {
            ImportScriptTemplates();
            ImportEditorTemplates();
            ImportFileTemplates();
            ImportShaderTemplates();
        }

        #endregion

        #region Settings and Help

        [MenuItem(menuItem + "Folder Settings", priority = 31)]
        private static void ProjectViewSettings()
        {
            //var path = $"{toolPath}Editor/Default Setup Folders.asset";
            //if (!File.Exists(path)) { return; }
            //var asset = AssetDatabase.LoadAssetAtPath<Object>(path);

            EditorGUIUtility.PingObject(preferenceData);
            Selection.activeObject = preferenceData;
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

        #endregion

        #region Create and Copy Functions
        private static string DestinationPath()
        {
            var destinationPath = Path.Combine(Application.dataPath, "ScriptTemplates");
            Directory.CreateDirectory(destinationPath);

            return destinationPath;
        }

        public static void CreateDirectories(string root, params string[] dir)
        {
            var fullPath = Path.Combine(Application.dataPath, root);
            foreach (var newDirectory in dir)
            {
                Directory.CreateDirectory(Path.Combine(fullPath, newDirectory));
            }
        }

        [InitializeOnLoadMethod]
        private static void FindPreferences()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(SetupFoldersPreference)}");
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                preferenceData = AssetDatabase.LoadAssetAtPath<SetupFoldersPreference>(path);
            }
        }

        private static void CopyFiles(string sourcePath, string destinationPath)
        {
            var dataPath = Application.dataPath.Replace("Assets", @"Packages\com.kiltec.setuptool");
            string[] filePaths = Directory.GetFiles($@"{dataPath}\{sourcePath}\");

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

            Debug.LogWarning($"Unity must be restarted to use the new {sourcePath}!");
            AssetDatabase.Refresh();
        }

        #endregion

        private enum UnityRenderer
        {
            SRP,
            HDRP,
            LWRP,
            URP
        }
    }
}