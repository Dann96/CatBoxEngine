using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

namespace GameEventSystem
{
    public class CutsceneCommand : GameEventCommand
    {
        public bool pauseStateMachine = false;
        public PlayableDirector director;

        CinemachineBrain brain;
        CinemachineVirtualCameraBase vCam;

        public override void InEditorUpdate(GameObject go)
        {
            if (director)
            {
                Debug.DrawLine(go.transform.position, director.transform.position, Color.yellow);
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(director.transform.position, Vector3.one);
            }
            
        }


        protected IEnumerator ExecuteCoroutine()
        {
            if (pauseStateMachine)
            {
                brain = GameObject.FindObjectOfType<CinemachineBrain>();
                vCam = director.GetComponentInChildren<CinemachineVirtualCameraBase>();
                vCam.MoveToTopOfPrioritySubqueue();
                while (!CinemachineCore.Instance.IsLive(vCam) || brain.IsBlending)
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            director.Play();
            while ((director.time + 0.05f) <= director.duration)
            {
                yield return new WaitForEndOfFrame();
            }
            if (pauseStateMachine)
            {
                CameraController.instance.virtualCamera.MoveToTopOfPrioritySubqueue();
                while (!CinemachineCore.Instance.IsLive(CameraController.instance.virtualCamera) || brain.IsBlending)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            
            finished = true;
        }

        protected override void ExecuteImmediately()
        {
            mono.StartCoroutine(ExecuteCoroutine());
        }
    }
}