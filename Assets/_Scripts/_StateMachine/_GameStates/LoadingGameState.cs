using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameState : IState
{
    public void Enter(IStateContext context)
    {
        if (context is not GameStateManager state) return;

        Debug.Log("loading game stateEnum entered ");

        SceneManager.LoadScene(1);
    }


    public void Exit(IStateContext context)
    {
        if (context is not GameStateManager state) return;

        Debug.Log("Loading game stateEnum exit ");
    }
}