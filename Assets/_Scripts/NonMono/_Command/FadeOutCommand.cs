using Cysharp.Threading.Tasks;

public class FadeOutCommand : ICommand
{
    private readonly Chip _first;

    private readonly Chip _second;

    private readonly int _score;


    public FadeOutCommand(Chip first, Chip second)
    {
        _first = first;
        _second = second;

        //_score = GameData.GetScore(first, second);
    }


    public void Execute()
    {
        _first.SetState(Chip.States.LightOff);

        _second.SetState(Chip.States.LightOff);

        GameManager.Instance.AddScore(_score);
    }


    public async UniTask Undo()
    {
        _first.SetState(Chip.States.LightOn);

        _second.SetState(Chip.States.LightOn);

        CameraController.Instance.MoveToBoardPosition(_second.BoardPosition.y);

        GameManager.Instance.AddScore(-_score);

        await UniTask.Yield();
    }
}