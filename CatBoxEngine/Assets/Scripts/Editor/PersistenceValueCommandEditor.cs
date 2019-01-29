using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameEventSystem
{
    [CustomEditor(typeof(PersistenceValueCommand))]
    public class PersistenceValueCommandEditor : GameEventCommandEditor
    {
        PersistenceValueCommand pCom;
        bool toggle;
        int solVal;
        float decVal;
        string wordVal;

        protected override void Init()
        {
            pCom = command as PersistenceValueCommand;

            switch (pCom.structCast)
            {
                case GameEventCondition.StructCast.Bool:
                    toggle = pCom.value != string.Empty && pCom.value == "1";
                    break;
                case GameEventCondition.StructCast.Int:
                    solVal = int.Parse(pCom.value);
                    break;
                case GameEventCondition.StructCast.Float:
                    decVal = float.Parse(pCom.value);
                    break;
                case GameEventCondition.StructCast.String:
                    string wordVal = serializedObject.FindProperty("value").ToString();
                    break;
            }
        }

        protected override void DrawCommand()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("key"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("structCast"));
            
            switch (pCom.structCast)
            {
                case GameEventCondition.StructCast.Bool:
                    EditorGUILayout.BeginHorizontal();
                    toggle = EditorGUILayout.Toggle(toggle);
                    pCom.value = toggle ? "1" : "0";
                    EditorGUILayout.EndHorizontal();
                    break;
                case GameEventCondition.StructCast.Int:
                case GameEventCondition.StructCast.Float:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("keyInputType"));
                    switch (pCom.keyInputType)
                    {
                        case PersistenceValueCommand.KeyInputType.Increment:
                            break;
                        case PersistenceValueCommand.KeyInputType.Decrement:
                            break;
                        case PersistenceValueCommand.KeyInputType.SolidValue:
                            if (pCom.structCast == GameEventCondition.StructCast.Int)
                            {
                                solVal = EditorGUILayout.IntField(solVal);
                                pCom.value = solVal.ToString();
                            }
                            else
                            {
                                decVal = EditorGUILayout.FloatField(decVal);
                                pCom.value = decVal.ToString();
                            }
                            break;
                    }
                    break;
                case GameEventCondition.StructCast.String:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("value"));
                    break;
            }
            //EditorGUILayout.TextArea(pCom.value);
            DrawNextCommandVariable();
        }

        public override string GetFoldoutLabel()
        {
            return "Persistence Value";
        }
    }
}