using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem
{
    public class SpawnCommand : GameEventCommand
    {
        public GameObject prefab;
        public bool useTargetAsSpawn;
        public Transform spawnTarget;

        public Vector3 position;
        public Vector3 rotation;

        public bool includeRotation;

        public override void InEditorUpdate(GameObject go)
        {
            if (useTargetAsSpawn)
            {
                if (spawnTarget)
                {
                    Debug.DrawLine(go.transform.position, spawnTarget.position, Color.cyan);
                    Debug.DrawRay(spawnTarget.transform.position, Quaternion.Euler(rotation) * Vector3.forward);
                }
            }
            else
            {
                Debug.DrawLine(go.transform.position, position, Color.cyan);
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(position, Vector3.one);
                Debug.DrawRay(position, Quaternion.Euler(rotation) * Vector3.forward);
            }
        }

        protected override void ExecuteImmediately()
        {
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            if (useTargetAsSpawn)
            {
                pos = spawnTarget.position;
                if (includeRotation)
                    rot = spawnTarget.rotation;
            }
            else
            {
                pos = position;
                if (includeRotation)
                    rot = Quaternion.Euler(rotation);
            }
            Instantiate(prefab, pos, rot);
            finished = true;
        }
    }
}