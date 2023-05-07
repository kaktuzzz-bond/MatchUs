#define ENABLE_LOGS

public class PauseGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Logger.Debug("Pause Game State Entered ");
    }


    public void Exit(GameFiniteStateMachine context)
    {
        throw new System.NotImplementedException();
    }
}