using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameStateContext
{
    private IGameState _currentGameState = new PrepareGameState();


    private void Start()
    {
        Prewarm();
    }


    private void Prewarm() => _currentGameState.Prepare(this);


    private void Game() => _currentGameState.Active(this);


    private void Pause() => _currentGameState.Pause(this);


    private void Exit() => _currentGameState.Exit(this);


    public void SetState(IGameState newState) => _currentGameState = newState;
}