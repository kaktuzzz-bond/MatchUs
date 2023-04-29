using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameState : IGameState
{
    public void Enter(GameStateManager context)
    {
        Debug.Log("Loading Game State entered ");

        SceneManager.LoadSceneAsync(1);
    }
}