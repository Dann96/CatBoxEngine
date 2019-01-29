using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GameEventSystem
{
    [CustomEditor(typeof(SpawnCommand))]
    public class SpawnCommandEditor : GameEventCommandEditor
    {
        bool useTarget;
        bool useRotation;

        protected override void DrawCommand()
        {
            useTarget = serializedObject.FindProperty("useTargetAsSpawn").boolValue;
            useRotation = serializedObject.FindProperty("includeRotation").boolValue;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("prefab"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useTargetAsSpawn"));

            if (useTarget)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnTarget"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("includeRotation"));
                if (useRotation)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("rotation"));
                }
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("position"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("includeRotation"));
                if (useRotation)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("rotation"));
                }
            }
            

            DrawNextCommandVariable();
        }

        public override string GetFoldoutLabel()
        {
            return "Spawn Object";
        }
    }
}