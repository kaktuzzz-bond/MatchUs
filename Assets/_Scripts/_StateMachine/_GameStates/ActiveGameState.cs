using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveGameState : IState
{
    public void Enter(IStateContext context)
    {
        if (context is not GameStateMachine state) return;

        Debug.Log("Active game state entered ");

        Debug.LogWarning(
                $"Difficulty: ({GameManager.Instance.Difficulty}) | " +
                $"Chips number ({(int)GameManager.Instance.Difficulty}) ");

        SceneManager.LoadScene(2);
    }


    public void Exit(IStateContext context)
    {
        if (context is not GameStateMachine state) return;

        Debug.Log("Active game state exit ");
    }
}