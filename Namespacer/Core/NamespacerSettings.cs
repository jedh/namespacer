using System;
using UnityEditor;
using UnityEngine;

namespace Namespacer.Core
{
    [Serializable]
    public class NamespacerSettings : ScriptableObject
    {
        public string rootNamespace = "";

        public string rootFolder = "";

        public bool shouldIgnoreHierarchy = false;

        public void Init()
        {
            rootNamespace = PlayerSettings.productName;
        }
    }
}