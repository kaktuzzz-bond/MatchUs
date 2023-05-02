using UnityEngine;

public class FadeOutCommand : ICommand
{
    private readonly Chip _first;

    private readonly Chip _second;


    public FadeOutCommand(Chip first, Chip second)
    {
        _first = first;
        _second = second;
    }


    public void Execute()
    {
        _first.ChipStateManager.SetFadedOutState();

        _second.ChipStateManager.SetFadedOutState();
    }


    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}