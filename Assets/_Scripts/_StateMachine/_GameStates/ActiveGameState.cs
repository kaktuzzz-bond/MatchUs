using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveGameState : IGameState
{
    public void Enter(GameStateManager context)
    {
        Debug.Log("Active Game State entered ");

        DOTween.SetTweensCapacity(5000, 1000);
        
        SceneManager.LoadScene(2);
    }
}