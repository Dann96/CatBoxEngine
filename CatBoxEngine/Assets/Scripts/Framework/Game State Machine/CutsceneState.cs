using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneState : IGameState
{
    public CutsceneState() { }

    public void StateStart() { }

    public void StateUpdate() { }

    public void StateFixedUpdate() { }

    public void StateStop() { }

    public int GetState()
    {
        return 3;
    }
}
