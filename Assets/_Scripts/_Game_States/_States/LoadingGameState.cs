using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameState : IGameState
{
    public void Prepare(IGameStateContext context)
    {
        Debug.Log("Returning to Main Menu");

        SceneManager.LoadScene(0);

        context.SetState(new PrepareGameState());
    }


    public void Loading(IGameStateContext context) { }


    public void Active(IGameStateContext context)
    {
        Debug.Log("Game started with " + (int)GameManager.Instance.Difficulty + " chips");

        SceneManager.LoadScene(2);

        context.SetState(new ActiveGameState());
    }


    public void Pause(IGameStateContext context) { }


    public void Exit(IGameStateContext context) { }
}