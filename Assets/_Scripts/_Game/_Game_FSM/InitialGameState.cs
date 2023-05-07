#define ENABLE_LOGS

public class InitialGameState : IGameState
{
    private const int SceneIndex = 0;


    public void Enter(GameFiniteStateMachine context)
    {
        Logger.Debug("MainScreen: Initial Game State entered ");
    }


    public void Exit(GameFiniteStateMachine context)
    {
        throw new System.NotImplementedException();
    }
}