using UnityEngine;

public class OldExitGameState : IGameState
{
    public void Prepare(IGameStateContext context) { }


    public void Loading(IGameStateContext context)
    {
        Debug.Log("Saving on exit game");

        context.SetState(new OldLoadingGameState());
    }


    public void Active(IGameStateContext context) { }


    public void Pause(IGameStateContext context) { }


    public void Exit(IGameStateContext context) { }
}