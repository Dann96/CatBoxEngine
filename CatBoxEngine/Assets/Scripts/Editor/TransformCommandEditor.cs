using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameEventSystem
{
    [CustomEditor(typeof(TransformCommand))]
    public class TransformCommandEditor : GameEventCommandEditor
    {
        protected override void DrawCommand()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("transform"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("instantMove"));
            if (!serializedObject.FindProperty("instantMove").boolValue)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rotate"));
            
            if (serializedObject.FindProperty("rotate").boolValue)  
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateTo"));
                for (int i = 0; i < serializedObject.FindProperty("useRotationAxes").arraySize; i++)
                {
                    string axisName = string.Empty;
                    switch (i)
                    {
                        case 0:
                            axisName = "X";
                            break;
                        case 1:
                            axisName = "Y";
                            break;
                        case 2:
                            axisName = "Z";
                            break;
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("useRotationAxes").GetArrayElementAtIndex(i), new GUIContent(axisName));
                    if (serializedObject.FindProperty("useRotationAxes").GetArrayElementAtIndex(i).boolValue)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotationAxes").GetArrayElementAtIndex(i), new GUIContent("Amount"));
                    }
                }
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("translate"));
           
            if (serializedObject.FindProperty("translate").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("translateTo"));
                for (int i = 0; i < serializedObject.FindProperty("useTranslationAxes").arraySize; i++)
                {
                    string axisName = string.Empty;
                    switch (i)
                    {
                        case 0:
                            axisName = "X";
                            break;
                        case 1:
                            axisName = "Y";
                            break;
                        case 2:
                            axisName = "Z";
                            break;
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("useTranslationAxes").GetArrayElementAtIndex(i), new GUIContent(axisName));
                    if (serializedObject.FindProperty("useTranslationAxes").GetArrayElementAtIndex(i).boolValue)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("translationAxes").GetArrayElementAtIndex(i), new GUIContent("Amount"));
                    }
                }
            }
            DrawNextCommandVariable();
        }

        public override string GetFoldoutLabel()
        {
            return "Transform Orientation";
        }
    }
}