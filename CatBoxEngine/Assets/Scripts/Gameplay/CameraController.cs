using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    public float moveSpeed = 10.0f;
    public CameraConstraints cameraConstraints = new CameraConstraints();

    public Transform target;
    public CinemachineVirtualCamera virtualCamera;

    private Vector3 newPos;

    private void Start()
    {
        ReturnToCameraRig();
    }

    void FixedUpdate()
    {
        newPos = new Vector3(Mathf.Clamp(newPos.x, cameraConstraints.minX, cameraConstraints.maxX),
            Mathf.Clamp(newPos.y, cameraConstraints.minY, cameraConstraints.maxY),
            Mathf.Clamp(newPos.z, cameraConstraints.minZ, cameraConstraints.maxZ));

        //if (virtualCamera.prio == CameraState.)

        transform.position = Vector3.Lerp(transform.position,newPos,Time.deltaTime * moveSpeed);

        if (target)
            newPos = target.position;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void MoveToNewFollowTransform(CinemachineVirtualCameraBase newTarget)
    {
        newTarget.MoveToTopOfPrioritySubqueue();
    }

    public void ReturnToCameraRig()
    {
        MoveToNewFollowTransform(this.virtualCamera);
    }
}

[Serializable]
public class CameraConstraints
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;
}
