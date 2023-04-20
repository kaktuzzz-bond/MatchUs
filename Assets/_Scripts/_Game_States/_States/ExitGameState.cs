using UnityEngine;

public class ExitGameState : IGameState
{
    public void Prepare(IGameStateContext context) { }


    public void Loading(IGameStateContext context)
    {
        Debug.Log("Saving on exit game");

        context.SetState(new LoadingGameState());
    }


    public void Active(IGameStateContext context) { }


    public void Pause(IGameStateContext context) { }


    public void Exit(IGameStateContext context) { }
}