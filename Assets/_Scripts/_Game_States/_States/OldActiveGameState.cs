using UnityEngine;

public class OldActiveGameState : IGameState
{
    public void Prepare(IGameStateContext context) { }


    public void Loading(IGameStateContext context) { }


    public void Active(IGameStateContext context) { }


    public void Pause(IGameStateContext context)
    {
        Debug.Log("Pause");

        context.SetState(new OldPauseGameState());
    }


    public void Exit(IGameStateContext context) { }
}