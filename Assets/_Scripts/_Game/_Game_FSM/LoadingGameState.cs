#define ENABLE_LOGS
using UnityEngine.SceneManagement;

public class LoadingGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Logger.Debug("Loading Game State entered ");

        SceneManager.LoadSceneAsync(1);
    }
}