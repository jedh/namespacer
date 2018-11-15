using Namespacer.Core;
using System.IO;
using UnityEditor;
using UnityEngine;

public class NamespacerEditor : EditorWindow
{
    private NamespacerSettings _namespacerSettings;

    private bool _showSettings = false;

    private bool _showSettingsToggle = false;

    private bool _isInterface = false;

    private string _filename;

    private readonly string _settingsPath = "Assets/Resources/NamespacerSettings.asset";

    private bool _isFocused = false;

    private void Awake()
    {
        // Create the .asset object if it no longer exists (it's been deleted or moved).
        if (!File.Exists(_settingsPath))
        {
            Util.CreateSettingsAsset(_settingsPath);
            _showSettings = true;
        }

        _namespacerSettings = AssetDatabase.LoadAssetAtPath<NamespacerSettings>(_settingsPath);

        // If there is no namespace defined, we want to make sure we will prompt to show settings later.
        if (_namespacerSettings.rootNamespace == string.Empty)
        {
            _showSettings = true;
        }
    }

    void OnGUI()
    {
        if (_showSettings)
        {
            GUILayout.Label("Namespacer settings:", EditorStyles.boldLabel);
            _namespacerSettings.rootNamespace = EditorGUILayout.TextField("Root namespace: ", _namespacerSettings.rootNamespace);
            _namespacerSettings.rootFolder = EditorGUILayout.TextField("Root directory path: ", _namespacerSettings.rootFolder);
            GUILayout.Label("");
        }

        GUILayout.Label("Create namespaced script:", EditorStyles.boldLabel);
        GUI.SetNextControlName("filename");
        _filename = EditorGUILayout.TextField("Filename: ", _filename);

        _isInterface = GUILayout.Toggle(_isInterface, "Create as interface");

        GUILayout.Label("");

        if (GUILayout.Button("Create Script") ||
            (Event.current.isKey && Event.current.keyCode == KeyCode.Return))
        {
            NamespacerCore namespacerCore = new NamespacerCore(_namespacerSettings);
            if (namespacerCore.CreateScript(_filename, _isInterface))
            {
                this.Close();
            }            
        }

        GUILayout.Label("");

        if (!_showSettings)
        {
            _showSettingsToggle = EditorGUILayout.Foldout(_showSettingsToggle, "Namespacer settings:");
            if (_showSettingsToggle)
            {
                _namespacerSettings.rootNamespace = EditorGUILayout.TextField("Root namespace: ", _namespacerSettings.rootNamespace);
                _namespacerSettings.rootFolder = EditorGUILayout.TextField("Root directory path: ", _namespacerSettings.rootFolder);
            }
        }

        if (!_isFocused)
        {
            _isFocused = true;
            EditorGUI.FocusTextInControl("filename");
        }
    }

    [MenuItem("Assets/Create/Namespaced C# Script")]
    public static void CreateNamespaceScript()
    {
        EditorWindow.GetWindow(typeof(NamespacerEditor));
    }    
}
