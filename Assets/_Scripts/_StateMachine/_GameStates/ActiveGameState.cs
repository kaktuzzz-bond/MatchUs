using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveGameState : IGameState
{
    public void Enter(GameStateManager context)
    {
        Debug.Log("Active Game State entered ");

        SceneManager.LoadScene(2);
    }
}