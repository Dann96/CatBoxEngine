using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GameEventSystem
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : EditorWithSubEditors<GameEventCommandEditor, GameEventCommand>
    {
        private GameEvent gameEvent;
        private SerializedProperty conditionsProperty;
        private SerializedProperty commandsProperty;

        private Type[] commandTypes;
        private string[] commandTypeNames;
        private int selectedIndex;

        private const float dropAreaHeight = 50.0f;
        private const float controlSpacing = 5.0f;
        private const string conditionsPropName = "conditions";
        private const string commandsPropName = "commands";

        private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

        private void OnEnable()
        {
            gameEvent = (GameEvent)target;

            conditionsProperty = serializedObject.FindProperty(conditionsPropName);
            commandsProperty = serializedObject.FindProperty(commandsPropName);

            CheckAndCreateSubEditors(gameEvent.commands.ToArray());

            SetCommandNamesArray();
        }


        private void OnDisable()
        {
            CleanupEditors();
        }

        protected override void SubEditorSetup(GameEventCommandEditor editor)
        {
            //Debug.Log(editor);
            editor.commandsProperty = commandsProperty;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CheckAndCreateSubEditors(gameEvent.commands.ToArray());

            EditorGUILayout.PropertyField(serializedObject.FindProperty("trigger"));
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
           /* EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Trigger Type");
            gameEvent.trigger =  (TriggerType)EditorGUILayout.EnumPopup(gameEvent.trigger);
            EditorGUILayout.EndHorizontal();*/


            EditorGUILayout.BeginVertical(GUI.skin.box);
            for (int i = 0; i < conditionsProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                //serializedObject
                SerializedProperty prop1 = serializedObject.FindProperty("conditions").GetArrayElementAtIndex(i);
                SerializedObject propChild1 = new SerializedObject(prop1.objectReferenceValue);
                SerializedProperty propChildValue1 = propChild1.FindProperty("conditionKey");
                EditorGUILayout.PropertyField(propChildValue1);
                propChild1.ApplyModifiedProperties();

                SerializedProperty prop2 = serializedObject.FindProperty("conditions").GetArrayElementAtIndex(i);
                SerializedObject propChild2 = new SerializedObject(prop2.objectReferenceValue);
                SerializedProperty propChildValue2 = propChild2.FindProperty("conditionValue");
                EditorGUILayout.PropertyField(propChildValue2);
                propChild2.ApplyModifiedProperties();

                SerializedProperty prop3 = serializedObject.FindProperty("conditions").GetArrayElementAtIndex(i);
                SerializedObject propChild3 = new SerializedObject(prop3.objectReferenceValue);
                SerializedProperty propChildValue3 = propChild3.FindProperty("conditionComparer");
                EditorGUILayout.PropertyField(propChildValue3);
                propChild3.ApplyModifiedProperties();

                SerializedProperty prop4 = serializedObject.FindProperty("conditions").GetArrayElementAtIndex(i);
                SerializedObject propChild4 = new SerializedObject(prop4.objectReferenceValue);
                SerializedProperty propChildValue4 = propChild4.FindProperty("structCast");
                EditorGUILayout.PropertyField(propChildValue4);
                propChild4.ApplyModifiedProperties();

                EditorGUILayout.EndVertical();

                if (GUILayout.Button("X", GUILayout.MaxHeight(50)))
                {
                    conditionsProperty.RemoveFromObjectArrayAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add New Condition"))
            {
                GameEventCondition newCondition = GameEventCondition.CreateInstance<GameEventCondition>();
                conditionsProperty.AddToObjectArray(newCondition);
            }
            EditorGUILayout.EndVertical();


            for (int i = 0; i < subEditors.Length; i++)
            {
                string[] commandNames = new string[gameEvent.commands.Count + 1];
                int[] commandIDs = new int[gameEvent.commands.Count + 1];
                for (int k = 0; k < gameEvent.commands.Count; k++)
                {
                    if (k < i)
                    {
                        commandNames[k] = "Command ID: " + k;
                        commandIDs[k] = k;
                    }
                    else if (k > i)
                    {
                        commandNames[k - 1] = "Command ID: " + k;
                        commandIDs[k - 1] = k;
                    }
                }
                commandNames[gameEvent.commands.Count] = "End Event";
                commandIDs[gameEvent.commands.Count] = -1;
                //Debug.Log(subEditors[i]);
                subEditors[i].ProcessCurrentEventCommands(commandNames, commandIDs);
                serializedObject.Update();

                ////////////////////////
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();

                subEditors[i].showCommand = EditorGUILayout.Foldout(subEditors[i].showCommand, "#" + i.ToString() + ": " + subEditors[i].GetFoldoutLabel());

                if (GUILayout.Button("↑", GUILayout.Width(30)))
                {
                    if (i > 0)
                    {
                        SwapCommandIndexPositions(i, i - 1);
                    }
                }
                if (GUILayout.Button("↓", GUILayout.Width(30)))
                {
                    if (i < subEditors.Length-1)
                    {
                        SwapCommandIndexPositions(i, i + 1);
                    }
                }
                if (GUILayout.Button("X", GUILayout.Width(30)))
                {
                    commandsProperty.RemoveFromObjectArrayAt(i);
                }

                EditorGUILayout.EndHorizontal();

                if (subEditors[i].showCommand)
                {
                    subEditors[i].OnInspectorGUI();
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
                /////////////////////

                serializedObject.ApplyModifiedProperties();
            }

            if (gameEvent.commands.ToArray().Length > 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            Rect fullWidthRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(dropAreaHeight + verticalSpacing));

            Rect leftAreaRect = fullWidthRect;

            leftAreaRect.y += verticalSpacing * 0.5f;

            leftAreaRect.width *= 0.5f;
            leftAreaRect.width -= controlSpacing * 0.5f;

            leftAreaRect.height = dropAreaHeight;

            Rect rightAreaRect = leftAreaRect;

            rightAreaRect.x += rightAreaRect.width + controlSpacing;

            TypeSelectionGUI(leftAreaRect);

            serializedObject.ApplyModifiedProperties();
        }

        private void TypeSelectionGUI(Rect containingRect)
        {
            Rect topHalf = containingRect;
            topHalf.height *= 0.5f;
            Rect bottomHalf = topHalf;
            bottomHalf.y += bottomHalf.height;

            selectedIndex = EditorGUI.Popup(topHalf, selectedIndex, commandTypeNames);

            if (GUI.Button(bottomHalf, "Add Selected Command"))
            {
                Type commandType = commandTypes[selectedIndex];
                GameEventCommand newCommand = GameEventCommandEditor.CreateCommand(commandType);
                commandsProperty.AddToObjectArray(newCommand);
            }
        }

        private void SetCommandNamesArray()
        {
            Type commandType = typeof(GameEventCommand);

            Type[] allTypes = commandType.Assembly.GetTypes();

            List<Type> commandSubTypeList = new List<Type>();

            for (int i = 0; i < allTypes.Length; i++)
            {
                if (allTypes[i].IsSubclassOf(commandType) && !allTypes[i].IsAbstract)
                    commandSubTypeList.Add(allTypes[i]);
            }

            commandTypes = commandSubTypeList.ToArray();

            List<string> commandTypeNameList = new List<string>();

            for (int i = 0; i < commandTypes.Length; i++)
            {
                commandTypeNameList.Add(commandTypes[i].Name);
            }
            commandTypeNames = commandTypeNameList.ToArray();
        }

        private void SwapCommandIndexPositions(int index1, int index2)
        {
            GameEventCommand currentCommand = commandsProperty.GetArrayElementAtIndex(index1).objectReferenceValue as GameEventCommand;
            int currentNextI = currentCommand.nextCommand;
            GameEventCommand lastCommand = commandsProperty.GetArrayElementAtIndex(index2).objectReferenceValue as GameEventCommand;
            int lastNextI = lastCommand.nextCommand;

            currentCommand.nextCommand = lastNextI;
            lastCommand.nextCommand = currentNextI;

            commandsProperty.GetArrayElementAtIndex(index2).objectReferenceValue = currentCommand;
            commandsProperty.GetArrayElementAtIndex(index1).objectReferenceValue = lastCommand;

            GameEventCommandEditor currentEditor = subEditors[index1];
            GameEventCommandEditor lastEditor = subEditors[index2];
            subEditors[index2] = currentEditor;
            subEditors[index1] = lastEditor;
        }

        public void AddNewCondition()
        {

        }

        public void RemoveCondition()
        {

        }
    }
}