using Cysharp.Threading.Tasks;
using UnityEngine;

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

        _score = GameConfig.GetScore(first, second);

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
        Debug.LogWarning("Undo FADE OUT");
        
        await _first.ChipFiniteStateMachine.SetFadedInState();

       await _second.ChipFiniteStateMachine.SetFadedInState();

        GameManager.Instance.AddScore(-_score);
    }
}