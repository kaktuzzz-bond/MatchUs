using UnityEngine;

public class PauseGameState :  IState
{
    public void Enter(IStateContext context)
    {
        if (context is not GameStateManager state) return;

        Debug.Log("Pause game stateEnum entered ");
    }


    public void Exit(IStateContext context)
    {
        if (context is not GameStateManager state) return;

        Debug.Log("Pause game stateEnum exit ");
    }
}