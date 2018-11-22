using System;
using UnityEditor;
using UnityEngine;

namespace Namespacer.Core
{
    public class NamespacerCore
    {
        private NamespacerSettings _namespacerSettings;

        public NamespacerCore(NamespacerSettings namespacerSettings)
        {
            _namespacerSettings = namespacerSettings;
        }

        public bool CreateScript(string filename, bool isInterface)
        {
            try
            {
                if (!Util.IsValidFilename(filename))
                {
                    EditorUtility.DisplayDialog("Can't Create Script", "Invalid characters in filename.", "Ok");
                    return false;
                }

                if (!Util.IsValidNamespace(_namespacerSettings.rootNamespace))
                {
                    EditorUtility.DisplayDialog("Can't Create Script", "Invalid characters in namespace.", "Ok");
                    return false;
                }

                string path = Util.GetDirectoryPath(Selection.activeObject);

                if (path == string.Empty)
                {
                    // If for whatever reason we have an empty file path, this is considered an error.
                    EditorUtility.DisplayDialog("Can't Create Script", "Make sure to select a valid directory.", "Ok");
                    return false;
                }

                if (Util.IsExistingFile(filename, path) &&
                    !EditorUtility.DisplayDialog("Existing File", "A file with this name already exists.", "Overwrite", "Cancel"))
                {
                    return false;
                }

                // Format the namespace.    
                string namespaceStr = Util.GetNamespace(path, _namespacerSettings);

                ScriptBuilder.BuildScript(filename, path, namespaceStr, isInterface);              

                return true;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                return false;
            }
        }
    }
}
