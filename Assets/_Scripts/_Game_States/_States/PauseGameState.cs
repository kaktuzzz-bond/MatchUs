using UnityEngine;

public class PauseGameState : IGameState
{
    public void Prepare(IGameStateContext context) { }


    public void Loading(IGameStateContext context) { }


    public void Active(IGameStateContext context)
    {
        Debug.Log("Return after pause");
        context.SetState(new ActiveGameState());
    }


    public void Pause(IGameStateContext context) { }


    public void Exit(IGameStateContext context)
    {
        Debug.Log("Exit");
        context.SetState(new ExitGameState());
    }
}