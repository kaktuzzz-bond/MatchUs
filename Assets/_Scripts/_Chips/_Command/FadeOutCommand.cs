using Cysharp.Threading.Tasks;

public class FadeOutCommand : ICommand
{
    private readonly Chip _first;

    private readonly Chip _second;

    private readonly ChipController _chipController;

    private readonly int _score;


    public FadeOutCommand(Chip first, Chip second)
    {
        _first = first;
        _second = second;

        _score = GameData.GetScore(first, second);

        _chipController = ChipController.Instance;
    }


    public void Execute()
    {
        _first.ChipFiniteStateMachine.SetFadedOutState().Forget();

        _second.ChipFiniteStateMachine.SetFadedOutState().Forget();

        GameManager.Instance.AddScore(_score);
    }


    public async UniTask Undo()
    {
        await _first.ChipFiniteStateMachine.SetFadedInState();

        await _second.ChipFiniteStateMachine.SetFadedInState();

        CameraController.Instance.MoveToBoardPosition(_second.BoardPosition.y);

        GameManager.Instance.AddScore(-_score);
    }
}