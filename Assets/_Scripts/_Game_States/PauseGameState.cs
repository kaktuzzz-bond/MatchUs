public class PauseGameState : IGameState
{
    public void Prepare(IGameStateContext context)
    {
        context.SetState(new PrepareGameState());
    }


    public void Active(IGameStateContext context)
    {
        context.SetState(new ActiveGameState());
    }


    public void Pause(IGameStateContext context) { }


    public void Exit(IGameStateContext context)
    {
        context.SetState(new ExitGameState());
    }
}