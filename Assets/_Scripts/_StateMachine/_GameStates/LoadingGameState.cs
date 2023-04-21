using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameState : IState
{
    public void Enter(IStateContext context)
    {
        if (context is not GameStateMachine state) return;

        Debug.Log("loading game state entered ");

        SceneManager.LoadScene(1);
    }


    public void Exit(IStateContext context)
    {
        if (context is not GameStateMachine state) return;

        Debug.Log("Loading game state exit ");
    }
}