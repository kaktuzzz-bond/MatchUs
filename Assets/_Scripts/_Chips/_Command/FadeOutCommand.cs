using UnityEngine;

public class FadeOutCommand : ICommand
{
    private readonly Chip _first;

    private readonly Chip _second;

    private readonly ChipController _chipController;


    public FadeOutCommand(Chip first, Chip second)
    {
        _first = first;
        _second = second;

        _chipController = ChipController.Instance;
    }


    public void Execute()
    {
        _first.ChipStateManager.SetFadedOutState();

        _second.ChipStateManager.SetFadedOutState();

        _chipController.Log.AddCommand(this);
    }


    public void Undo()
    {
        _first.ChipStateManager.SetFadedInState();

        _second.ChipStateManager.SetFadedInState();
    }
}