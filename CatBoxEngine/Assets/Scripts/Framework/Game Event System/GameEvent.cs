using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Playables;

namespace GameEventSystem
{
    public class GameEvent : MonoBehaviour
    {
        public bool resetAfterCompletion;
        public TriggerType trigger;
        public List<GameEventCondition> conditions = new List<GameEventCondition>();
        public List<GameEventCommand> commands = new List<GameEventCommand>();

        private int eventIndex = 0;
        private bool eventActive = false;
        private bool interactable;

        private void Awake()
        {
            for (int i = 0; i < commands.Count; i++)
            {
                commands[i].Init();
            }
        }

        private void Start()
        {
            if (trigger == TriggerType.OnStart)
            {
                ProcessEvent();
            }
            else if (trigger == TriggerType.OnLevelLoadComplete)
            {
                GameManager.instance.onLevelFullyLoaded += ProcessEvent;
            }
        }

        private void Update()
        {
            if (interactable)
            {
                if (!eventActive)
                {

                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position, Vector3.one);
            for (int i = 0; i < commands.Count; i++)
            {
                commands[i].InEditorUpdate(gameObject);
            }
        }
#endif

        private void OnDestroy()
        {
            if (trigger == TriggerType.OnLevelLoadComplete)
            {
                GameManager.instance.onLevelFullyLoaded -= ProcessEvent;
            }
        }

        private bool ProcessConditions()
        {
            int validConditions = 0;

            Dictionary<string, string> persistenceValues = PlayerDataManager.instance.PlayerData.CurrentFile.gamePersistenceValues;

            for (int i = 0; i < conditions.Count; i++)
            {
                if (persistenceValues.ContainsKey(conditions[i].conditionKey))
                {
                    switch (conditions[i].conditionComparer)
                    {
                        case GameEventCondition.ConditionComparer.EqualTo:
                            switch (conditions[i].structCast)
                            {
                                case GameEventCondition.StructCast.Bool:
                                    if (bool.Parse(persistenceValues[conditions[i].conditionKey]) == bool.Parse(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                                case GameEventCondition.StructCast.Int:
                                    if (int.Parse(persistenceValues[conditions[i].conditionKey]) == int.Parse(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                                case GameEventCondition.StructCast.Float:
                                    if (float.Parse(persistenceValues[conditions[i].conditionKey]) == float.Parse(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                                case GameEventCondition.StructCast.String:
                                    if (persistenceValues[conditions[i].conditionKey] == conditions[i].conditionValue)
                                        validConditions++;
                                    break;
                            }
                            break;
                        case GameEventCondition.ConditionComparer.GreaterOrEqualTo:
                            switch (conditions[i].structCast)
                            {
                                case GameEventCondition.StructCast.Int:
                                    if (int.Parse(persistenceValues[conditions[i].conditionKey]) >= int.Parse(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                                case GameEventCondition.StructCast.Float:
                                    if (float.Parse(persistenceValues[conditions[i].conditionKey]) >= float.Parse(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                                case GameEventCondition.StructCast.String:
                                    break;
                            }
                            break;
                        case GameEventCondition.ConditionComparer.LessOrEqualTo:
                            switch (conditions[i].structCast)
                            {
                                case GameEventCondition.StructCast.Int:
                                    if (int.Parse(persistenceValues[conditions[i].conditionKey]) <= int.Parse(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                                case GameEventCondition.StructCast.Float:
                                    if (float.Parse(persistenceValues[conditions[i].conditionKey]) <= float.Parse(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                                case GameEventCondition.StructCast.String:
                                    break;
                            }
                            break;
                        case GameEventCondition.ConditionComparer.GreaterThan:
                            switch (conditions[i].structCast)
                            {
                                case GameEventCondition.StructCast.Int:
                                    if (int.Parse(persistenceValues[conditions[i].conditionKey]) > int.Parse(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                                case GameEventCondition.StructCast.Float:
                                    if (float.Parse(persistenceValues[conditions[i].conditionKey]) > float.Parse(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                                case GameEventCondition.StructCast.String:
                                    break;
                            }
                            break;
                        case GameEventCondition.ConditionComparer.LessThan:
                            switch (conditions[i].structCast)
                            {
                                case GameEventCondition.StructCast.Int:
                                    if (int.Parse(persistenceValues[conditions[i].conditionKey]) < int.Parse(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                                case GameEventCondition.StructCast.Float:
                                    if (float.Parse(persistenceValues[conditions[i].conditionKey]) < float.Parse(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                                case GameEventCondition.StructCast.String:
                                    break;
                            }
                            break;
                        case GameEventCondition.ConditionComparer.Contains:
                            switch (conditions[i].structCast)
                            {
                                default:
                                    if (persistenceValues[conditions[i].conditionKey].Contains(conditions[i].conditionValue))
                                        validConditions++;
                                    break;
                            }
                            break;
                    }
                }
            }
            if (commands != null && commands.Count != 0)
                validConditions++;
            return (validConditions == conditions.Count + 1);
        }

        public void ProcessEvent()
        {
            if (GameManager.FullyInitialized)
            {
                if (ProcessConditions())
                {
                    StartCoroutine(RunEventCoroutine());
                }
            }
            
        }

        private IEnumerator RunEventCoroutine()
        {
            eventActive = true;
            while (eventIndex >= 0)
            {
                commands[eventIndex].Execute(this);
                while (!commands[eventIndex].IsFinished)
                {
                    yield return new WaitForFixedUpdate();
                }
                commands[eventIndex].CleanUp();
                eventIndex = commands[eventIndex].nextCommand;
            }
            eventActive = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (trigger == TriggerType.EventTrigger)
            {
                ProcessEvent();
            }
            else if (trigger == TriggerType.PlayerTrigger)
            {
                if (other.CompareTag("Player"))
                {
                    ProcessEvent();
                }
            }
            else if (trigger == TriggerType.OnInteract)
            {
                if (other.CompareTag("Player"))
                {
                    interactable = true;
                }
            }

        }
    }

    public enum TriggerType
    {
        OnStart,
        OnLevelLoadComplete,
        EventTrigger,
        PlayerTrigger,
        OnInteract,
    }
}
