using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

namespace GameEventSystem
{
    [Serializable]
    public abstract class GameEventCommand : ScriptableObject
    {
        protected bool finished = false;
        public int nextCommand = -1;
        protected MonoBehaviour mono;

        public bool IsFinished
        {
            get { return finished; }
        }

        public void Init()
        {
            finished = false;
            SpecificInit();
        }

        public abstract void InEditorUpdate(GameObject go);

        public virtual void SpecificInit(){}

        public void Execute(MonoBehaviour monoBehaviour)
        {
            mono = monoBehaviour;
            ExecuteImmediately();
        }

        protected abstract void ExecuteImmediately();


        public virtual void CleanUp()
        {
            finished = false;
        }
    }
}