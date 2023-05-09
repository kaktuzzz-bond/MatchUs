using UnityEngine;

public class InitialGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Debug.Log("MainScreen: Initial Game State entered ");
    }
}