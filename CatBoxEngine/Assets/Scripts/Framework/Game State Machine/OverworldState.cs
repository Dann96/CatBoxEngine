using UnityEngine;

public class OverworldState : IGameState
{
    public OverworldState() { }

    public void StateStart() { }

    public void StateUpdate() { }

    public void StateFixedUpdate()
    {
        
        if (CharacterMotor.Player != null)
            CharacterMotor.Player.RelayInput((Camera.main.transform.right * Input.GetAxis("Horizontal")) 
                + (Camera.main.transform.forward * Input.GetAxis("Vertical")));
    }

    public void StateStop() { }

    public int GetState()
    {
        return 2;
    }
}