using Cysharp.Threading.Tasks;
using UnityEngine;

public class PauseGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Debug.Log("Pause Game State Entered ");
    }


}