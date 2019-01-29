using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Events;

namespace GameEventSystem
{
    public abstract class GameEventCommandEditor : Editor
    {
        public bool showCommand;
        public SerializedProperty commandsProperty;

        public GameEventCommand command;

        protected string[] commandNames;
        protected int[] commandIDs;

        private void OnEnable()
        {
            command = (GameEventCommand)target;

            Init();
        }

        protected virtual void Init() { }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawCommand();
            serializedObject.ApplyModifiedProperties();
        }

        public static GameEventCommand CreateCommand(Type commandType)
        {
            return (GameEventCommand)CreateInstance(commandType);
        }

        protected virtual void DrawCommand()
        {
            DrawDefaultInspector();
        }

        public void ProcessCurrentEventCommands(string[] names, int[] ids)
        {
            commandNames = names;
            commandIDs = ids;
        }

        public void DrawNextCommandVariable()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Next Command:");
            command.nextCommand = EditorGUILayout.IntPopup(command.nextCommand, commandNames, commandIDs);
            EditorGUILayout.EndHorizontal();
        }

        public abstract string GetFoldoutLabel();
    }
}
