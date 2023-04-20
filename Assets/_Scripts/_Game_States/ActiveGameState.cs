public class ActiveGameState : IGameState
{
    public void Prepare(IGameStateContext context) { }


    public void Active(IGameStateContext context) { }


    public void Pause(IGameStateContext context)
    {
        context.SetState(new PauseGameState());
    }


    public void Exit(IGameStateContext context)
    {
        context.SetState(new ExitGameState());
    }
}