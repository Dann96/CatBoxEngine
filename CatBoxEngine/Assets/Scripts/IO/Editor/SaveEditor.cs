using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveEditor : EditorWindow
{
    public PersistentData persistentData;
    Vector2 scrollPos;

    [MenuItem("Window/Save Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(SaveEditor)).Show();
    }

    private void OnGUI()
    {
        if (persistentData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("persistentData");
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.PropertyField(serializedProperty, true);
            EditorGUILayout.EndScrollView();
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save Data"))
                SaveGameData();

        }

        if (GUILayout.Button("Load Data"))
            LoadGameData();

        if (GUILayout.Button("Create new Data"))
            CreateNewGameData();

        if (GUILayout.Button("Delete Data"))
            DeleteGameData();
    }

    private void SaveGameData()
    {
        Debug.Log("Saving data to: " + PlayerDataManager.GetFilePath());

        string dataString = JsonUtility.ToJson(persistentData, true);
        using (StreamWriter writer = PlayerDataManager.GetWriteStream())
        {
            writer.Write(dataString);
        }
    }

    private void LoadGameData()
    {
        if (File.Exists(PlayerDataManager.GetFilePath()))
        {
            using (StreamReader reader = PlayerDataManager.GetReadStream())
            {
                persistentData = new PersistentData();
                JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), persistentData);
            }
        }
        else Debug.Log("Save file does not exist");
    }

    private void CreateNewGameData()
    {
        persistentData = new PersistentData();
    }

    private void DeleteGameData()
    {
        if (File.Exists(PlayerDataManager.GetFilePath()))
        {
            File.Delete(PlayerDataManager.GetFilePath());
            Debug.Log("Save file found and deleted");
        }
        else Debug.Log("Save file does not exist");
    }
}