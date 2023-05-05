#define ENABLE_LOGS
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CommandLogger
{
    public Stack<ICommand> Stack { get; } = new();

    private readonly WaitForSeconds _wait = new(0.2f);


    public void AddCommand(ICommand command)
    {
        Logger.Debug($"Adding {command}");
        Stack.Push(command);
    }


    public IEnumerator UndoCommand()
    {
        if (Stack.Count == 0)
        {
            Logger.Debug("Stack is empty");

            yield break;
        }

        // Add
        // Fade Out
        // Remove Single Line 
        // Special
        ICommand command;

        do
        {
            command = Stack.Pop();

            Logger.Debug($"Undo {command}");

            command.Undo();

            yield return _wait;
        } while (command.GetType() == typeof(RemoveSingleLineCommand));
    }
}