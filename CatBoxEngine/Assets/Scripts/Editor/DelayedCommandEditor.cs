using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameEventSystem
{
    [CustomEditor(typeof(DelayedCommand))]
    public class DelayedCommandEditor : GameEventCommandEditor
    {
        protected override void DrawCommand()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));

            DrawNextCommandVariable();
        }

        public override string GetFoldoutLabel()
        {
            return "Delay";
        }
    }
}