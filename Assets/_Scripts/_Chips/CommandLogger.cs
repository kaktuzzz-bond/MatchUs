#define ENABLE_LOGS
using System.Collections.Generic;
using UnityEngine;

public class CommandLogger
{
    public Stack<ICommand> Stack { get; } = new();


    public void AddCommand(ICommand command)
    {
        Logger.Debug($"Adding {command}");
        Stack.Push(command);
    }


    public void UndoCommand()
    {
        if (Stack.Count == 0)
        {
            Logger.Debug("Stack is empty");

            return;
        }

        ICommand command = Stack.Pop();

        Logger.Debug($"Undo {command}");

        command.Undo();
        
        // ICommand command;
        // do
        // {
        //     command = Stack.Pop();
        //
        //     Logger.Debug($"Undo {command}");
        //
        //     command.Undo();
        // } while (command.GetType() != typeof(FadeOutCommand));

    }
}