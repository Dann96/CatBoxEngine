using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public PatrolPath path;
    private CharacterMotor m_CharacterMotor;

    private int pathIndex = 0;

	void Start ()
    {
        m_CharacterMotor = GetComponent<CharacterMotor>();
        GotToNextPosition();
    }
	
	void Update ()
    {
        if (path)
        {
            if (path.pathPositions.Length > 0)
            {
                if (!m_CharacterMotor.pathPending && m_CharacterMotor.remainingDistance < 0.5f)
                {
                    GotToNextPosition();
                }
                m_CharacterMotor.RelayInput(m_CharacterMotor.AgentVelocity);
            }
        }
    }

    void GotToNextPosition()
    {
        m_CharacterMotor.SetAgentDestination(path.pathPositions[pathIndex]);

        pathIndex = (pathIndex + 1) % path.pathPositions.Length;
    }
}
