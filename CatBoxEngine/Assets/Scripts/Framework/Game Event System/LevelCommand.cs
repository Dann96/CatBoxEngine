using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem
{
    public class LevelCommand : GameEventCommand
    {
        public string sceneName;

        public override void InEditorUpdate(GameObject go)
        {
            
        }

        protected override void ExecuteImmediately()
        {
            GameManager.instance.LoadLevel(sceneName);
            finished = true;
        }
    }
}