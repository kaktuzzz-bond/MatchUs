using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandLogger
{
    public Stack<ICommand> Stack { get; } = new();

    private readonly WaitForSeconds _wait = new(0.2f);



    public void ExecuteAndAdd(ICommand command)
    {
        AddCommand(command);
        
        command.Execute();
    }
    
    
    public void AddCommand(ICommand command)
    {
        Debug.Log($"Adding {command}");
        Stack.Push(command);
    }


    public IEnumerator UndoCommand()
    {
        if (Stack.Count == 0)
        {
            Debug.Log("Stack is empty");

            yield break;
        }

        ICommand command;

        do
        {
            command = Stack.Pop();

            Debug.Log($"Undo {command}");

            command.Undo();

            yield return _wait;
        } while (command.GetType() == typeof(RemoveSingleLineCommand));
    }
}