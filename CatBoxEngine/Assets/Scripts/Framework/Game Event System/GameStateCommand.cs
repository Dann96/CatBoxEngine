using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameEventSystem
{
    [Serializable]
    public class GameStateCommand : GameEventCommand
    {
        public int stateIndex;

        public override void InEditorUpdate(GameObject go){}

        protected override void ExecuteImmediately()
        {
            GameManager.SetState(GameManager.GetState(stateIndex));
           
            finished = true;
        }
    }
}