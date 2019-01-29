// <summary>
// The state to be used for controlling the player's input during loading (just in case we want something)
// </summary>
public class LoadingState : IGameState
{
    public LoadingState() { }

    public void StateStart() { }

    public void StateUpdate() { }

    public void StateFixedUpdate() { }

    public void StateStop() { }

    public int GetState()
    {
        return 0;
    }
}