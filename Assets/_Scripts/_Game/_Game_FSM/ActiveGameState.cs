#define ENABLE_LOGS
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Logger.Debug("Active Game State entered ");

        DOTween.SetTweensCapacity(5000, 1000);

        GameFiniteStateMachine.Instance.LoadScene(2);

        GameFiniteStateMachine.Instance.OnSceneLoaded += DoOnLoad;

    }
    
    private static void DoOnLoad()
    {
        Logger.DebugWarning("ACTIVE");
            
        GameSceneGUI.Instance.Init();
        
        GameFiniteStateMachine.Instance.OnSceneLoaded -= DoOnLoad;
    }
    
}