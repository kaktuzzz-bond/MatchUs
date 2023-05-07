#define ENABLE_LOGS
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Logger.Debug("Loading Game State entered ");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
    }


    public void Exit(GameFiniteStateMachine context)
    {
        Logger.Debug("Loading Game State exit");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);

        if (asyncLoad.isDone)
        {
            GameFiniteStateMachine.Instance.Active();
        }
    }
}