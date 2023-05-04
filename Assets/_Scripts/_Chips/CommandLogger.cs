using System.Collections.Generic;
using UnityEngine;

public class CommandLogger
{
    public Stack<ICommand> Stack { get; } = new();


    public void AddCommand(ICommand command)
    {
        Debug.Log($"Adding {command}");
        Stack.Push(command);
    }


    public void UndoCommand()
    {
        if (Stack.Count == 0)
        {
            Debug.Log("Stack is empty");
            return;
        }

        ICommand   command = Stack.Pop();

        Debug.Log($"Undo {command}");

        command.Undo();
    }
}