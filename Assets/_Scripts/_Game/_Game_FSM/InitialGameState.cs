using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialGameState : IGameState
{
    
    private const int LoadingSceneIndex = 0;
    
    public void Enter(GameFiniteStateMachine context)
    {
        Debug.Log("MainScreen: Initial Game State entered ");
        
        LoadAsync().Forget();
    }


    public void Exit(GameFiniteStateMachine context)
    {
        //throw new System.NotImplementedException();
    }
    
    
    private static async UniTaskVoid LoadAsync()
    {
        Debug.Log("Active Game State entered ");

        await SceneManager.LoadSceneAsync(LoadingSceneIndex);
    }
}