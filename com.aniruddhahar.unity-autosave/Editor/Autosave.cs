// Unity-Autosave by Aniruddha Hardikar | https://github.com/aniruddhahar
// This package depends on the Editor Coroutines package. 
// Please ensure the Editor Coroutines package in installed in the Package Manager.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.PackageManager.UI;
using Unity.EditorCoroutines.Editor;
using UnityEngine.UIElements;
using System.Reflection.Emit;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;


[InitializeOnLoad]
public class Autosave : EditorWindow
{
    static string descriptionText = "Autosave requires the Editor Coroutines package to be installed." +
        "\nPlease ensure the Editor Coroutines package in installed in the Package Manager,"
        +"\nor click the button below to Install or Update it.";

    static string infoText = "After changing Autosave Settings, Please make sure to click Update Interval!";

    static int saveInterval = 10;
    static bool autoSaveEnabled = true, logSaves = true, saveOnPlay = false;
    static bool showSettings;

    static EditorCoroutine saveRoutine;
    static AutosavePrefs prefs;
    static Autosave windowInstance;

    [InitializeOnLoadMethod]
    private static void OnEnable()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Autosave")) AssetDatabase.CreateFolder("Assets", "Autosave");
        if (prefs == null) {prefs = CreateInstance<AutosavePrefs>(); SavePrefs(); }
        
        LoadPrefs();
        
        EditorApplication.playModeStateChanged += OnPlayModeToggle;

        if (saveRoutine == null) saveRoutine = EditorCoroutineUtility.StartCoroutineOwnerless(SaveOpenScenesCoroutine());

    }

    private static void OnDisable()
    {
        SavePrefs();
        EditorApplication.playModeStateChanged -= OnPlayModeToggle;
    }

    [MenuItem("Autosave/Autosave Settings")]
    static void Init()
    {

        windowInstance = (Autosave)EditorWindow.GetWindow(typeof(Autosave));
        windowInstance.Show();
        prefs = AssetDatabase.LoadAssetAtPath<AutosavePrefs>("Assets/Autosave/Resources/autosaveprefs.asset");
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.HelpBox(descriptionText, MessageType.Info);
        if (GUILayout.Button("Install/Update Editor Coroutines Package"))
        {
            InstallEditorCoroutines();
        }
        EditorGUILayout.EndVertical();

        showSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showSettings, "Autosave Settings");
        if (showSettings)
        {
            EditorGUILayout.BeginVertical();
            autoSaveEnabled = EditorGUILayout.Toggle("Enable Auto Save", autoSaveEnabled);
            logSaves = EditorGUILayout.Toggle("Log in console on save", logSaves);
            saveOnPlay = EditorGUILayout.Toggle("Save on Play", saveOnPlay);
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.BeginHorizontal();

        saveInterval = EditorGUILayout.IntField("Save Interval in Minutes", saveInterval);
        if (GUILayout.Button("Update Interval"))
        {
            if (saveRoutine != null) EditorCoroutineUtility.StopCoroutine(saveRoutine);
            saveRoutine = EditorCoroutineUtility.StartCoroutineOwnerless(SaveOpenScenesCoroutine());
            SavePrefs();
        }

        if (GUILayout.Button("Save NOW"))
        {
            SaveAllOpenScenes();
            Debug.Log("Saved open scenes at: " + System.DateTime.Now);
            SavePrefs();
        }

        // Tests for saving and loading Autsave Prefs from scriptable Object.

        //if (GUILayout.Button("Load Prefs"))
        //{

        //    loadPrefs();
        //    if (saveRoutine != null) EditorCoroutineUtility.StopCoroutine(saveRoutine);
        //    saveRoutine = EditorCoroutineUtility.StartCoroutineOwnerless(SaveOpenScenesCoroutine());

        //}


        //if (GUILayout.Button("Save Prefs"))
        //{
        //    savePrefs();
        //    if (saveRoutine != null) EditorCoroutineUtility.StopCoroutine(saveRoutine);
        //    saveRoutine = EditorCoroutineUtility.StartCoroutineOwnerless(SaveOpenScenesCoroutine());
        //}

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox(infoText, MessageType.Info);

    }

    // Coroutine that handles the autosave execution
    static IEnumerator SaveOpenScenesCoroutine()
    {
        int interval = saveInterval*60;
        var waitForSave = new EditorWaitForSeconds(interval);

        while (true)
        {

            if (autoSaveEnabled && !Application.isPlaying)
            {
                SaveAllOpenScenes();
            }

            yield return waitForSave;

        }
    }

    static void SaveAllOpenScenes()
    {
        SavePrefs();
        EditorSceneManager.SaveOpenScenes();
        

        if (logSaves) Debug.Log("Saved open scenes at: " + System.DateTime.Now);

    }

    static void OnPlayModeToggle(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {

            SavePrefs();

            if (saveOnPlay)
            {
                SaveAllOpenScenes();
                Debug.Log("Saved on play at: " + System.DateTime.Now);
            }
            
        }

        if (state == PlayModeStateChange.ExitingPlayMode)
        {

            LoadPrefs();
            if (saveRoutine != null) EditorCoroutineUtility.StopCoroutine(saveRoutine);
            saveRoutine = EditorCoroutineUtility.StartCoroutineOwnerless(SaveOpenScenesCoroutine());
        }
    }

    static void LoadPrefs()
    {
        prefs = AssetDatabase.LoadAssetAtPath<AutosavePrefs>("Assets/Autosave/autosaveprefs.asset");

        if (prefs == null)
        {

            prefs = CreateInstance<AutosavePrefs>();
            AssetDatabase.CreateAsset(prefs, "Assets/Autosave/autosaveprefs.asset");

        }

        //Debug.Log(prefs.interval);
        autoSaveEnabled = prefs.enable;
        saveInterval = prefs.interval;
        saveOnPlay = prefs.onPlay;
        logSaves = prefs.log;

    }

    static void SavePrefs()
    {

        if (prefs == null)
        {

            prefs = CreateInstance<AutosavePrefs>();
            AssetDatabase.CreateAsset(prefs, "Assets/Autosave/autosaveprefs.asset");

        }

        prefs.enable = autoSaveEnabled;
        prefs.interval = saveInterval;
        prefs.onPlay = saveOnPlay;
        prefs.log = logSaves;

        EditorUtility.SetDirty(prefs);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();


    }

    void InstallEditorCoroutines() {

        Client.Add("com.unity.editorcoroutines");
        Debug.Log("Installing/Updating Editor Coroutines.");
    
    }

}


