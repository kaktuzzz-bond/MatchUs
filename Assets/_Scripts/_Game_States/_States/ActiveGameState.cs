using UnityEngine;

public class ActiveGameState : IGameState
{
    public void Prepare(IGameStateContext context) { }


    public void Loading(IGameStateContext context) { }


    public void Active(IGameStateContext context) { }


    public void Pause(IGameStateContext context)
    {
        Debug.Log("Pause");

        context.SetState(new PauseGameState());
    }


    public void Exit(IGameStateContext context) { }
}