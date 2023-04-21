using UnityEngine;

public class OldGameStateMachine : Singleton<OldGameStateMachine>, IGameStateContext
{
    public bool EnterGame { get; private set; } = true;

    private IGameState _currentGameState = new OldPrepareGameState();


    public void Prepare()
    {
        EnterGame = true;

        _currentGameState.Prepare(this);
    }


    public void LoadData()
    {
        _currentGameState.Loading(this);
    }


    public void MakeGameStateActive()
    {
        _currentGameState.Active(this);
    }


    public void PauseGame()
    {
        _currentGameState.Pause(this);
    }


    public void ExitGame()
    {
        EnterGame = false;

        _currentGameState.Exit(this);
    }


    public void SetState(IGameState newState)
    {
        _currentGameState = newState;
    }
}