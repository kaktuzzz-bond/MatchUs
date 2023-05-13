using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PauseGameState : IGameState
{
    public event Action<bool> OnPauseGameStateChanged;
            
    
    public void Enter(GameFiniteStateMachine context)
    {
        Debug.Log("Pause Game State Entered ");
        
        OnPauseGameStateChanged?.Invoke(false);
    }


    public void Exit(GameFiniteStateMachine context)
    {
        Debug.Log("Resume The Game");
        
        OnPauseGameStateChanged?.Invoke(true);
    }
}