using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialGameState : IGameState
{
    private const int SceneIndex = 0;


    public void Enter(GameStateManager context)
    {
        Debug.Log("MainScreen: Initial Game State entered ");

        // if (SceneManager.GetActiveScene().buildIndex != SceneIndex)
        // {
        //     SceneManager.LoadScene(SceneIndex);
        // }
    }
}