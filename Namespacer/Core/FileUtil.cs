using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Namespacer.Core
{
    public partial class Util
    {
        public static bool IsValidFilename(string filename)
        {
            string pattern = @"['{}[\]\\;':"",./? !@#$%&*()+=-]";

            return !Regex.IsMatch(filename, pattern);
        }

        public static string GetDirectoryPath(UnityEngine.Object activeObject)
        {
            string path = string.Empty;

            if (activeObject == null)
            {
                path = "Assets";
            }
            else
            {
                path = AssetDatabase.GetAssetPath(activeObject.GetInstanceID());

                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }
            }

            return path;
        }

        public static bool IsExistingFile(string filename, string directoryPath)
        {
            bool isExisting = false;
            string fullFilePath = directoryPath + "/" + filename + ".cs";

            if (File.Exists(fullFilePath))
            {
                isExisting = true;
            }

            return isExisting;
        }

        public static void CreateSettingsAsset(string settingsPath)
        {
            NamespacerSettings settings = ScriptableObject.CreateInstance<NamespacerSettings>();
            settings.Init();

            if (!Directory.Exists("Assets/Resources"))
            {
                Directory.CreateDirectory("Assets/Resources");
            }

            AssetDatabase.CreateAsset(settings, settingsPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
