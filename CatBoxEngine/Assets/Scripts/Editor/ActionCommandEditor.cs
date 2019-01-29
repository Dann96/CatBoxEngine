using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace GameEventSystem
{
    [CustomEditor(typeof(ActionCommand))]
    public class ActionCommandEditor : GameEventCommandEditor
    {
        public UnityEvent action;

        protected override void DrawCommand()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("action"));

            DrawNextCommandVariable();
        }

        public override string GetFoldoutLabel()
        {
            return "Action";
        }
    }
}