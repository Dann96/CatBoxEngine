/// <summary>
/// The base interface for the various game states.
/// </summary>
public interface IGameState
{
    /// <summary>
    ///Called when the state is called to run.
    /// </summary>
    void StateStart();
    // <summary>
    ///Called on Update.
    /// </summary>
    void StateUpdate();
    // <summary>
    ///Called on FixedUpdate.
    /// </summary>
    void StateFixedUpdate();
    // <summary>
    ///Called when the state is swapped out with another.
    /// </summary>
    void StateStop();
    // <summary>
    ///Used to get quick state reference.
    /// </summary>
    int GetState();
}