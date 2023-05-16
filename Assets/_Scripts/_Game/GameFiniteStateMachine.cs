
public class GameFiniteStateMachine
{
    public IGameState CurrentGameState { get; private set; }

    public bool IsExitGame { get; private set; }

    private readonly InitialGameState _initial = new();

    private readonly LoadingGameState _loading = new();

    private readonly ActiveGameState _active = new();

    private readonly PauseGameState _pause = new();


    public GameFiniteStateMachine()
    {
        CurrentGameState = _initial;
    }


    public void Initial()
    {
        IsExitGame = false;

        SetState(_initial);
    }


    public void Loading()
    {
        SetState(_loading);
    }


    public void Active()
    {
        SetState(_active);

        IsExitGame = true;
    }


    public void Pause()
    {
        SetState(_pause);
    }


    private void SetState(IGameState newGameState)
    {
        CurrentGameState.Exit(this);

        CurrentGameState = newGameState;

        CurrentGameState.Enter(this);
    }
}