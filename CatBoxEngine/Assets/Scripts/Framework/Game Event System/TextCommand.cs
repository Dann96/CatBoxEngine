using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameEventSystem
{
    [Serializable]
    public class TextCommand : GameEventCommand
    {
        [Multiline]
        public string text;

        public DialogueFlavor dialogueFlavor;

        public bool useCustomOrientation;

        public TextAnchor textAnchor;

        public Vector3 position;

        public Vector2 anchorMinimum;

        public Vector2 anchorMaximum;



        public override void InEditorUpdate(GameObject go) { }

        protected IEnumerator ExecuteCoroutine()
        {
            while (GUIManager.instance.IsTypingText)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }

        protected override void ExecuteImmediately()
        {
            mono.StartCoroutine(ExecuteCoroutine());
        }
    }
}