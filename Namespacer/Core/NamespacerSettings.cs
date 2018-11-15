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

        public void Init()
        {
            rootNamespace = PlayerSettings.productName;
        }
    }
}