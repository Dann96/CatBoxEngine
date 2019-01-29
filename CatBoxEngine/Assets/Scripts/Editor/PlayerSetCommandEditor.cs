using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GameEventSystem
{
    [CustomEditor(typeof(PlayerSetCommand))]
    public class PlayerSetCommandEditor : GameEventCommandEditor
    {

        protected override void DrawCommand()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("characterTag"));
            DrawNextCommandVariable();
        }

        public override string GetFoldoutLabel()
        {
            return "Player Assignment";
        }
    }
}