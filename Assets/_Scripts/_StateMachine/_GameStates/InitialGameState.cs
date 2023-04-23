using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialGameState : IState
{
    public void Enter(IStateContext context)
    {
        if (context is not GameStateManager state) return;

        Debug.Log("Initial game stateEnum entered ");
    }


    public void Exit(IStateContext context)
    {
        if (context is not GameStateManager state) return;

        Debug.Log("Initial game stateEnum exit ");
    }
}