using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PauseGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Debug.Log("Pause Game State Entered ");

        GameGUI.Instance.SetButtonPressPermission(false);
        
        GameManager.Instance.DisableTimer();
        
    }


    public void Exit(GameFiniteStateMachine context)
    {
        Debug.Log("Resume The Game");

        GameGUI.Instance.SetButtonPressPermission(true);
        
        GameManager.Instance.EnableTimer();
    }
}