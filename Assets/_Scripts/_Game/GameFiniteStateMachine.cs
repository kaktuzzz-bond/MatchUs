using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFiniteStateMachine : Singleton<GameFiniteStateMachine>
{
    public event Action OnSceneLoaded;
    
    [ShowInInspector]
    public IGameState CurrentGameState { get; private set; }


    private void Start()
    {
        Initial();
    }


    private void Initial()
    {
        
        SetState(new InitialGameState());
    }


    public void Loading()
    {
        SetState(new LoadingGameState());
    }


    public void Active()
    {
        SetState(new ActiveGameState());
    }


    public void Pause()
    {
        SetState(new PauseGameState());
    }


    public void Exit()
    {
        SetState(new ExitGameState());
    }


    private void SetState(IGameState newGameState)
    {
        CurrentGameState = newGameState;

        CurrentGameState.Enter(this);
    }


    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneRoutine(sceneIndex));
    }


    private IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        OnSceneLoaded?.Invoke();
    }
}