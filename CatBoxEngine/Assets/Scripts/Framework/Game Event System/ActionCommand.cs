using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace GameEventSystem
{
    [Serializable]
    public class ActionCommand : GameEventCommand
    {
        public UnityEvent action;

        public override void InEditorUpdate(GameObject go)
        {
            for (int i = 0; i < action.GetPersistentEventCount(); i++)
            {
                GameObject ob = action.GetPersistentTarget(i) as GameObject;
                if (go && ob)
                    Debug.DrawLine(go.transform.position, ob.transform.position, Color.green);
            }
        }

        protected override void ExecuteImmediately()
        {
            if (action != null)
                action.Invoke();

            finished = true;
        }
    }
}
