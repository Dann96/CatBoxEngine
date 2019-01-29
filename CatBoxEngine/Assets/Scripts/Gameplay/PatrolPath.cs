using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    private const float POSITION_RADIUS = 0.5f;
    public Vector3[] pathPositions;

    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < pathPositions.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(pathPositions[i], POSITION_RADIUS);
            if (pathPositions.Length > 1)
            {
                if (i < pathPositions.Length - 1)
                    Debug.DrawLine(pathPositions[i], pathPositions[i + 1]);
                else Debug.DrawLine(pathPositions[i], pathPositions[0]);
            }
        }
    }

    void Update()
    {
        
    }
}
