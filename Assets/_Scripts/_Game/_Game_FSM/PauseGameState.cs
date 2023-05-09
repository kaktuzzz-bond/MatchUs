using UnityEngine;

public class PauseGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Debug.Log("Pause Game State Entered ");
    }


    public void Exit(GameFiniteStateMachine context)
    {
        throw new System.NotImplementedException();
    }
}