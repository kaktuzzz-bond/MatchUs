using UnityEngine;

public class PauseGameState :  IState
{
    public void Enter(IStateContext context)
    {
        if (context is not GameStateMachine state) return;

        Debug.Log("Pause game state entered ");
    }


    public void Exit(IStateContext context)
    {
        if (context is not GameStateMachine state) return;

        Debug.Log("Pause game state exit ");
    }
}