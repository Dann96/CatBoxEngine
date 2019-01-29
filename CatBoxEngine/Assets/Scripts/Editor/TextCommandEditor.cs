using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;

namespace GameEventSystem
{
    public class TextCommandEditor : GameEventCommandEditor
    {
        bool useCustom;

        protected override void DrawCommand()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("text"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogueFlavor"));

            useCustom = serializedObject.FindProperty("useCustomOrientation").boolValue;

            if (useCustom)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("position"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("anchorMinimum"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("anchorMaximum"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("textAnchor"));
            }
            DrawNextCommandVariable();
        }

        public override string GetFoldoutLabel()
        {
            return "Text";
        }
    }
}
