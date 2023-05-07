#define ENABLE_LOGS

public class InitialGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Logger.Debug("MainScreen: Initial Game State entered ");
    }
    
}