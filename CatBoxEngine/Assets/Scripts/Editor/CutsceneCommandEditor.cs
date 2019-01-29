using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameEventSystem
{
    [CustomEditor(typeof(CutsceneCommand))]
    public class CutsceneCommandEditor : GameEventCommandEditor
    {
        protected override void DrawCommand()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("director"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pauseStateMachine"));

            DrawNextCommandVariable();
        }

        public override string GetFoldoutLabel()
        {
            return "Cutscene";
        }
    }
}
