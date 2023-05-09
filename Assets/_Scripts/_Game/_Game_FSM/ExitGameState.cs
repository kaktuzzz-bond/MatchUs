using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Debug.Log("Exit Game State (Save data) entered ");

        SceneManager.LoadScene(0);
    }


    public void Exit(GameFiniteStateMachine context)
    {
        throw new System.NotImplementedException();
    }
}