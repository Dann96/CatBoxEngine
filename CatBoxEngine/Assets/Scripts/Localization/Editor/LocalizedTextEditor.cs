using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace Localization
{
    public class LocalizedTextEditor : EditorWindow
    {
        public LocalizationData localizationData;
        Vector2 scrollPos;

        [MenuItem("Window/Localized Text Editor")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(LocalizedTextEditor)).Show();
        }

        private void OnGUI()
        {
            if (localizationData != null)
            {
                SerializedObject serializedObject = new SerializedObject(this);
                SerializedProperty serializedProperty = serializedObject.FindProperty("localizationData");
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                EditorGUILayout.PropertyField(serializedProperty, true);
                EditorGUILayout.EndScrollView();
                serializedObject.ApplyModifiedProperties();

                if (GUILayout.Button("Save Data"))
                    SaveLocalizationData();

                if (GUILayout.Button("Sort Data"))
                    SortLocalizationData();
            }

            if (GUILayout.Button("Load Data"))
                LoadLocalizationData();

            if (GUILayout.Button("Create new Data"))
                CreateNewLocalizationData();
        }

        private void LoadLocalizationData()
        {
            string filePath = EditorUtility.OpenFilePanel("Select Localization Data File", Application.streamingAssetsPath, "json");


            if (!string.IsNullOrEmpty(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                localizationData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            }
        }

        private void SaveLocalizationData()
        {
            string filePath = EditorUtility.SaveFilePanel("Save Localization Data File", Application.streamingAssetsPath, "", "json");

            if (!string.IsNullOrEmpty(filePath))
            {
                string dataAsJson = JsonUtility.ToJson(localizationData, true);
                File.WriteAllText(filePath, dataAsJson);
            }
        }

        private void SortLocalizationData()
        {
            if (localizationData.items.Length > 0)
            {
                localizationData.items = localizationData.items.OrderBy(item => item.key).ToArray();
            }
        }

        private void CreateNewLocalizationData()
        {
            localizationData = new LocalizationData();
        }
    }
}