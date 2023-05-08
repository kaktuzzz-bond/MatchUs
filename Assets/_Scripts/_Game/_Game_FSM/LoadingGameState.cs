#define ENABLE_LOGS
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Logger.Debug("Loading Game State entered ");

        GameFiniteStateMachine.Instance.LoadScene(1);

        GameFiniteStateMachine.Instance.OnSceneLoaded += DoOnLoad;
    }


    private static void DoOnLoad()
    {
        GameFiniteStateMachine.Instance.OnSceneLoaded -= DoOnLoad;
    }
}