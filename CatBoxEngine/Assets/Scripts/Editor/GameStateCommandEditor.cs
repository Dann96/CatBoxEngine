using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GameEventSystem
{
    [CustomEditor(typeof(GameStateCommand))]
    public class GameStateCommandEditor : GameEventCommandEditor
    {
        Type[] stateTypes;
        string[] stateNames;

        protected override void Init()
        {
            GetStatesArray();
        }

        protected override void DrawCommand()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stateIndex"));
            /*EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Popup(serializedObject.FindProperty("stateIndex").intValue, stateNames);
            EditorGUILayout.EndHorizontal();

            serializedObject.FindProperty("stateIndex").intValue = EditorGUI.Popup(new Rect(0, 0, 20, 10),
                serializedObject.FindProperty("stateIndex").intValue, stateNames);*/
            DrawNextCommandVariable();
        }

        public override string GetFoldoutLabel()
        {
            return "State Change";
        }

        private void GetStatesArray()
        {
            Type stateType = typeof(IGameState);

            Type[] allStates = stateType.Assembly.GetTypes();

            List<Type> stateSubTypeList = new List<Type>();

            for (int i = 0; i < allStates.Length; i++)
            {
                if (allStates[i].IsSubclassOf(stateType) && !allStates[i].IsAbstract)
                    stateSubTypeList.Add(allStates[i]);
            }

            stateTypes = stateSubTypeList.ToArray();

            List<string> stateTypeNameList = new List<string>();

            for (int i = 0; i < stateTypes.Length; i++)
            {
                stateTypeNameList.Add(stateTypes[i].Name);
            }
            stateNames = stateTypeNameList.ToArray();
        }
    }
}