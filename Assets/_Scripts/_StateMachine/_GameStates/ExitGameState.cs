using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameState : IGameState
{
    public void Enter(GameStateManager context)
    {
        Debug.Log("Exit Game State (Save data) entered ");

        SceneManager.LoadScene(0);
    }
}