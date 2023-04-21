using UnityEngine;

public class ExitGamState : IState
{
    public void Enter(IStateContext context)
    {
        if (context is not GameStateMachine state) return;

        Debug.Log("Exit game state entered ");
    }


    public void Exit(IStateContext context)
    {
        if (context is not GameStateMachine state) return;

        //Save data here
        Debug.Log("Exit game state exit ");
    }
}