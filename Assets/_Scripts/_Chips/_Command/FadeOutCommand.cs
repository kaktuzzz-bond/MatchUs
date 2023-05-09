using System;

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
        _first.ChipFiniteStateMachine.SetFadedOutState();

        _second.ChipFiniteStateMachine.SetFadedOutState();

        GameManager.Instance.AddScore(_score);
        
        _chipController.Log.AddCommand(this);
    }


    public void Undo()
    {
        _first.ChipFiniteStateMachine.SetFadedInState();

        _second.ChipFiniteStateMachine.SetFadedInState();
        
        GameManager.Instance.AddScore(-_score);
    }
}