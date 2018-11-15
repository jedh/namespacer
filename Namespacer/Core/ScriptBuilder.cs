using Namespacer.Templates;
using System.IO;
using UnityEditor;

namespace Namespacer.Core
{
    public class ScriptBuilder
    {
        public static void BuildScript(string filename, string path, string namespaceStr, bool isInterface)
        {
            string destPath = GetDestinationPath(filename, path);
            string scriptString;

            if (isInterface)
            {
                scriptString = GetStandardFormatInterface(filename, namespaceStr, destPath);
            }
            else
            {
                scriptString = GetStandardFormatScript(filename, namespaceStr, destPath);
            }            

            File.WriteAllText(destPath, scriptString);

            // Uncomment if we decide to allow stock Unity formatting.
            // TBH though, if you care about namespaces, you probably don't care about Unity's default MonoBehaviour formatting.
            //if (namespacerSettings.shouldUseUnityFormatting)
            //{
            //    BuildUnityFormatScript(filename, namespaceStr, destPath);
            //}
            //else
            //{
            //    BuildStandardFormatScript(filename, namespaceStr, destPath);
            //}

            AssetDatabase.Refresh();
        }

        private static string GetDestinationPath(string filename, string path)
        {
            string destPath = path + "/" + filename + ".cs";

            return destPath;
        }

        private static string GetStandardFormatScript(string filename, string namespaceStr, string destPath)
        {
            MBStandardTextTemplate mbScript = new MBStandardTextTemplate() { FileName = filename, NamespaceString = namespaceStr };
            string mbString = mbScript.TransformText();

            return mbString;            
        }

        private static string GetStandardFormatInterface(string filename, string namespaceStr, string destPath)
        {
            InterfaceStandardTextTemplate standardInterface = new InterfaceStandardTextTemplate() { FileName = filename, NamespaceString = namespaceStr };
            string interfaceString = standardInterface.TransformText();

            return interfaceString;
        }

        //private static void BuildUnityFormatScript(string filename, string namespaceStr, string destPath)
        //{
        //}
    }
}
