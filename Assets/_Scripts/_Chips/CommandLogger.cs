#define ENABLE_LOGS
using System;
using System.Collections;
using System.Collections.Generic;
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


    public static void ExecuteCommand(ICommand command)
    {
        ICommand execute = command ?? throw new ArgumentNullException(nameof(command));

        execute.Execute();
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