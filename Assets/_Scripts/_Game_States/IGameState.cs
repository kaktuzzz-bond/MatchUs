public interface IGameState
{
    void Prepare(IGameStateContext context);


    void Active(IGameStateContext context);


    void Pause(IGameStateContext context);


    void Exit(IGameStateContext context);
}