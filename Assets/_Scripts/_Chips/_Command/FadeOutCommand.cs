using System;
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


    public event Action OnUndoCompleted;


    public void Execute()
    {
        _first.ChipFiniteStateMachine.SetFadedOutState();

        _second.ChipFiniteStateMachine.SetFadedOutState();

        _chipController.Log.AddCommand(this);
    }


    public void Undo()
    {
        _first.ChipFiniteStateMachine.SetFadedInState();

        _second.ChipFiniteStateMachine.SetFadedInState();
        
        OnUndoCompleted?.Invoke();
    }
}