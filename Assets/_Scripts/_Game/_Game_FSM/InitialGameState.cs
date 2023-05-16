using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Debug.Log("MainScreen: Initial Game State entered ");
    }


    public void Exit(GameFiniteStateMachine context)
    {
        SceneManager.LoadSceneAsync(1);
    }
}