using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameEventSystem
{
    [Serializable]
    public class DelayedCommand : GameEventCommand
    {
        public float delay;

        protected WaitForSeconds wait;

        public override void InEditorUpdate(GameObject go)
        {
            
        }

        public override void SpecificInit()
        {
            wait = new WaitForSeconds(delay);
        }

        protected IEnumerator ExecuteCoroutine()
        {
            yield return wait;
            finished = true;
        }

        protected override void ExecuteImmediately()
        {
            mono.StartCoroutine(ExecuteCoroutine());
        }
    }
}