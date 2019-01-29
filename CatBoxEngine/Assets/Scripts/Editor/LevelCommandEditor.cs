using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameEventSystem
{
    [CustomEditor(typeof(LevelCommand))]
    public class LevelCommandEditor : GameEventCommandEditor
    {
        protected override void DrawCommand()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sceneName"));

            DrawNextCommandVariable();
        }

        public override string GetFoldoutLabel()
        {
            return "Level Change";
        }
    }
}