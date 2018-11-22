using System.Text.RegularExpressions;
using UnityEditor;


namespace Namespacer.Core
{
    public partial class Util
    {
        public static bool IsValidNamespace(string rootNamespace)
        {
            //string pattern = @"[/?<>\:*|,.+-`'{}&%^#!=@;~]";
            string pattern = @"['{}[\]\\;':"",/?!@#$%&*()+=-]";

            return !Regex.IsMatch(rootNamespace, pattern);
        }

        public static string GetNamespace(string path, NamespacerSettings namespacerSettings)
        {
            string namespacePath = "";
            string rootNamespace = namespacerSettings.rootNamespace;

            if (rootNamespace == string.Empty)
            {
                rootNamespace = PlayerSettings.productName;
            }
           
            // Strip any whitespace out of the root namespace.
            rootNamespace = rootNamespace.Replace(" ", "");

            if (namespacerSettings.shouldIgnoreHierarchy)
            {
                namespacePath = rootNamespace;
            }
            else
            {
                namespacePath = GetNamespaceFromHierarchy(path, rootNamespace, namespacerSettings.rootFolder);
            }                      

            return namespacePath;
        }

        private static string GetNamespaceFromHierarchy(string path, string rootNamespace, string rootFolder)
        {
            string namespacePath = path;

            // Get the root directory.
            // Check if Assets/ is at the start of the path, and remove it.
            // It should always be but the check is here just in case.
            if (namespacePath.IndexOf("Assets") == 0)
            {
                namespacePath = path.Remove(0, 6);
            }

            // If we have a leading /, remove it.
            if (namespacePath.IndexOf("/") == 0)
            {
                namespacePath = namespacePath.Remove(0, 1);
            }

            // Remove leading and trailing / from the root folders setting.
            string rootFolderString = rootFolder;

            // If we have a leading /, remove it.
            if (rootFolderString.IndexOf("/") == 0)
            {
                rootFolderString = rootFolderString.Remove(0, 1);
            }

            // If We have a trailing /, remove it.
            if (rootFolderString.IndexOf("/") == rootFolderString.Length - 1)
            {
                rootFolderString = rootFolderString.Remove(rootFolderString.Length - 1, 1);
            }

            var pathFolders = namespacePath.Split('/');
            var rootFolders = rootFolderString.Split('/');

            // Check if the root folder exists in an exact match.
            // The root directory path assumes it comes immediately after Assets/, it doesn't search
            // for the first occurrence of it.     
            if (namespacePath.IndexOf(rootFolderString) == 0)
            {
                int replaceLength = 0;

                for (int i = 0; i < rootFolders.Length; i++)
                {
                    // Make sure there's an item in pathFolders.
                    if ((pathFolders.Length - 1) >= i)
                    {
                        if (rootFolders[i] == pathFolders[i])
                        {
                            // Add the index counter to the length to account for the '/' we split on.
                            replaceLength += rootFolders[i].Length + i;
                        }
                        else
                        {
                            replaceLength = 0;
                            break;
                        }
                    }
                }

                namespacePath = namespacePath.Remove(0, replaceLength);
            }

            // If we have a leading /, remove it.
            if (namespacePath.IndexOf("/") == 0)
            {
                namespacePath = namespacePath.Remove(0, 1);
            }

            if (namespacePath.Length > 0)
            {
                // If We have a trailing /, remove it.
                if (namespacePath.IndexOf("/") == namespacePath.Length - 1)
                {
                    namespacePath = namespacePath.Remove(namespacePath.Length - 1, 1);
                }

                namespacePath = namespacePath.Replace("/", ".");
                namespacePath = rootNamespace + "." + namespacePath;
            }
            else
            {
                namespacePath = rootNamespace;
            }

            return namespacePath;
        }
    }
}
