using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ConfinedCameraLocation : MonoBehaviour
{
    [Range(0f,1f)]public float intensity = 0.0f;
    public CinemachineVirtualCamera focusPoint;
    private CinemachineMixingCamera mixingCamera;

    private void Start()
    {
        mixingCamera = GetComponent<CinemachineMixingCamera>();
    }

    private void Update()
    {
        mixingCamera.SetWeight(0, intensity);
        mixingCamera.SetWeight(1, 1f - intensity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.instance.MoveToNewFollowTransform(mixingCamera);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.instance.ReturnToCameraRig();
        }
    }
}
