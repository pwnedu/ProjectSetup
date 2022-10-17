using UnityEngine;
using UnityEditor;
using System.IO;

namespace SetupTool
{
    public class KeywordReplace : UnityEditor.AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            int index = path.LastIndexOf(".");
            if (index < 0) { return; }

            string file = path.Substring(index);
            if (file != ".cs" && file != ".js" && file != ".boo") { return; }

            string myPath = path;
            int folderIndex = myPath.LastIndexOf("/");
            char[] charArray = myPath.ToCharArray();
            string[] newstring = myPath.Split(charArray[folderIndex]);

            string fileName = newstring[newstring.Length - 1];
            int extensionIndex = fileName.IndexOf(".");
            char[] charArray2 = fileName.ToCharArray();
            string[] myName = fileName.Split(charArray2[extensionIndex]);

            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;
            if (!File.Exists(path)) { return; }

            string fileContent = File.ReadAllText(path);

            fileContent = fileContent.Replace("17/10/2022 14:16:06", System.DateTime.Now + "");
            fileContent = fileContent.Replace("My project", PlayerSettings.productName);
            fileContent = fileContent.Replace("DefaultCompany", PlayerSettings.companyName);

            string noEditor = myName[0].Replace("Editor", "");
            fileContent = fileContent.Replace("KeywordReplace", noEditor);

            File.WriteAllText(path, fileContent);
            AssetDatabase.Refresh();
        }
    }
}