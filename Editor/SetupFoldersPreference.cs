using UnityEngine;

namespace SetupTool
{
    [CreateAssetMenu(menuName = "Unity Tools/My Setup/Preferences", order = 10)]
    public class SetupFoldersPreference : ScriptableObject
    {
        public string projectFolder = "_MyProject";
        public string[] subFolderNames = new string[7] { "Models", "Materials", "Prefabs", "Scripts", "Scenes", "Shaders", "Textures" };
    }
}