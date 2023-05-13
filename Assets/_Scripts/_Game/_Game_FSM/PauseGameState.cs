using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PauseGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Debug.Log("Pause Game State Entered ");

        InputManager.Instance.DisablePlayerInput();
        
        GameManager.Instance.DisableTimer();
        
    }


    public void Exit(GameFiniteStateMachine context)
    {
        Debug.Log("Resume The Game");

        InputManager.Instance.EnablePlayerInput();
        
        GameManager.Instance.EnableTimer();
    }
}