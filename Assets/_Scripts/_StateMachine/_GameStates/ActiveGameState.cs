using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveGameState : IState
{
    public void Enter(IStateContext context)
    {
        if (context is not GameStateManager state) return;

        Debug.Log("Active game stateEnum entered ");

        Debug.LogWarning(
                $"Difficulty: ({GameManager.Instance.Difficulty}) | " +
                $"Chips number ({(int)GameManager.Instance.Difficulty}) ");

        SceneManager.LoadScene(2);
    }


    public void Exit(IStateContext context)
    {
        if (context is not GameStateManager state) return;

        Debug.Log("Active game stateEnum exit ");
    }
}