using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandLogger
{
    private readonly Stack<ICommand> _stack = new();

    private readonly WaitForSeconds _wait = new(0.2f);



    public void ExecuteAndAdd(ICommand command)
    {
        AddCommand(command);
        
        command.Execute();
    }


    private void AddCommand(ICommand command)
    {
        Debug.Log($"Adding {command}");
        _stack.Push(command);
    }


    public IEnumerator UndoCommand()
    {
        if (_stack.Count == 0)
        {
            Debug.Log("Stack is empty");

            yield break;
        }

        ICommand command;

        do
        {
            command = _stack.Pop();

            Debug.Log($"Undo {command}");

            command.Undo();

            yield return _wait;
        } while (command.GetType() == typeof(RemoveSingleLineCommand));
    }
}