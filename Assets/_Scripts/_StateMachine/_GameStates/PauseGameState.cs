using UnityEngine;

public class PauseGameState : IGameState
{
    public void Enter(GameStateManager context)
    {
        Debug.Log("Pause Game State Entered ");
    }
}