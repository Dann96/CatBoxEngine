public class MenuState : IGameState
{
    public MenuState() { }

    public void StateStart()
    {
        GUIManager.instance.eventSystem.sendNavigationEvents = true;
    }

    public void StateUpdate() { }

    public void StateFixedUpdate() { }

    public void StateStop()
    {
        GUIManager.instance.eventSystem.sendNavigationEvents = false;
    }

    public int GetState()
    {
        return 1;
    }
}
