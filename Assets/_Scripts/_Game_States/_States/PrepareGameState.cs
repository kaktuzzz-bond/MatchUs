using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareGameState : IGameState
{
    public void Prepare(IGameStateContext context) { }


    public void Loading(IGameStateContext context)
    {
        SceneManager.LoadScene(1);

        context.SetState(new LoadingGameState());
    }


    public void Active(IGameStateContext context) { }


    public void Pause(IGameStateContext context) { }


    public void Exit(IGameStateContext context) { }
}