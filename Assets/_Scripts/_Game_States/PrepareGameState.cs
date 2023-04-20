public class PrepareGameState : IGameState
{
    public void Prepare(IGameStateContext context) { }


    public void Active(IGameStateContext context)
    {
        context.SetState(new ActiveGameState());
    }


    public void Pause(IGameStateContext context) { }


    public void Exit(IGameStateContext context) { }
}