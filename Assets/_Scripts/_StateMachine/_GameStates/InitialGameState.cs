using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialGameState : IState
{
    public void Enter(IStateContext context)
    {
        if (context is not GameStateMachine state) return;

        Debug.Log("Initial game state entered ");
    }


    public void Exit(IStateContext context)
    {
        if (context is not GameStateMachine state) return;

        Debug.Log("Initial game state exit ");
    }
}