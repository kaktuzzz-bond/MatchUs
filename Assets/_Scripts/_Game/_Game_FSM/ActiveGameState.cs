using DG.Tweening;
using UnityEngine;

public class ActiveGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Debug.Log("Active Game State entered ");

        DOTween.SetTweensCapacity(5000, 1000);

        GameFiniteStateMachine.Instance.LoadScene(2);

        GameFiniteStateMachine.Instance.OnSceneLoaded += DoOnLoad;
    }


    private static void DoOnLoad()
    {
        GameGUI.Instance.Init();

        GameFiniteStateMachine.Instance.OnSceneLoaded -= DoOnLoad;
    }
}