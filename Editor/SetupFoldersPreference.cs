using UnityEngine;

namespace SetupTool
{
    [CreateAssetMenu(menuName = "Unity Tools/My Setup/Preferences", order = 10)]
    public class SetupFoldersPreference : ScriptableObject
    {
        public string projectFolder = "_MyProject";
        public string[] subFolders = new string[11] { "Editor", "Materials", "Models", "Prefabs", "Resources", "Scenes", "Scripts", "Settings", "Shaders", "Sounds", "Textures" };
        //OTHER FOLDERS: //"Gizmos", "Plugins", "StreamingAssets", "ScriptTemplates"//

        public FolderStructure[] structures;
    }

    [System.Serializable]
    public struct FolderStructure
    {
        public string assetFolderPath;
        public string[] subFolders;
    }
}