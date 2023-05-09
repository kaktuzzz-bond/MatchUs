using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Debug.Log("Exit Game State (Save data) entered ");

        DOTween.KillAll();
        
        SceneManager.LoadScene(0);
    }
}