using System;

public interface ICommand
{
    event Action OnUndoCompleted;
    void Execute();


    void Undo();
}