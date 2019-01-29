using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    public bool lerp = false;
    [Range(0f,1f)] public float point;
    public Transform target1;
    public Transform target2;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (lerp)
            transform.position = Vector3.Lerp(target1.position,target2.position, point);
        else transform.position = Vector3.MoveTowards(target1.position, target2.position, point);
    }
}
