using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameEventSystem
{
    [Serializable]
    public class PlayerSetCommand : GameEventCommand
    {
        public string characterTag;

        public override void InEditorUpdate(GameObject go) { }

        protected override void ExecuteImmediately()
        {
            foreach (CharacterMotor character in GameObject.FindObjectsOfType<CharacterMotor>())
            {
                if (character.characterTag == characterTag)
                {
                    character.SetAsPlayer();
                    break;
                }
                    
            }
            finished = true;
        }
    }
}
