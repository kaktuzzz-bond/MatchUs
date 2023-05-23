using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class CommandLogger
{
    private static readonly Stack<ICommand> Log = new();


    public static async UniTask AddCommand(ICommand command)
    {
        Log.Push(command);

        await command.Execute();

        CheckStackCount();
    }


    public static async UniTask UndoCommand()
    {
        if (Log.Count == 0)
        {
            Debug.Log("Log is empty!");

            return;
        }

        ICommand command;

        do
        {
            command = Log.Pop();

            await command.Undo();
        } while (command.GetType() == typeof(RemoveSingleLineCommand));

        GameGUI.Instance.HideInfo();

        CheckStackCount();
    }


    private static void CheckStackCount()
    {
        GameGUI.Instance.UndoButton.SetInteractivity(Log.Count > 1);
    }
}