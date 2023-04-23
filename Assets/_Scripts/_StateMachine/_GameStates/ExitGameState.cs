using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameState : IState
{
    public void Enter(IStateContext context)
    {
        if (context is not GameStateManager state) return;

        Debug.Log("Exit game stateEnum entered ");
    }


    public void Exit(IStateContext context)
    {
        if (context is not GameStateManager state) return;

        //Save data here
        Debug.Log("Exit game stateEnum exit ");
        SceneManager.LoadScene(0);
    }
}