using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialGameState : IGameState
{
    public void Enter(GameStateManager context)
    {
        Debug.Log("MainScreen: Initial Game State entered ");

        
    }
}