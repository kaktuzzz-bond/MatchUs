using System;

public class SpecialActionCommand : ICommand
{
    public event Action OnUndoCompleted;


    public void Execute()
    {
        throw new System.NotImplementedException();
    }


    public void Undo()
    {
        throw new System.NotImplementedException();
        OnUndoCompleted?.Invoke();
    }
}