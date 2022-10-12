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
            string file = path.Substring(index);

            string myPath = path;
            int folderIndex = myPath.LastIndexOf("/");
            char[] charArray = myPath.ToCharArray();
            string[] newstring = myPath.Split(charArray[folderIndex]);

            string fileName = newstring[newstring.Length - 1];
            int extensionIndex = fileName.IndexOf(".");
            char[] charArray2 = fileName.ToCharArray();
            string[] myName = fileName.Split(charArray2[extensionIndex]);

            if (file != ".cs" && file != ".js" && file != ".boo") return;
            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;

            file = File.ReadAllText(path);

            file = file.Replace("#CREATION_DATE#", System.DateTime.Now + "");
            file = file.Replace("#PROJECT_NAME#", PlayerSettings.productName);
            file = file.Replace("#DEVELOPER_NAME#", PlayerSettings.companyName);

            string noEditor = myName[0].Replace("Editor", "");
            file = file.Replace("#PRIMARY_SCRIPTNAME#", noEditor);

            File.WriteAllText(path, file);
            AssetDatabase.Refresh();
        }
    }
}