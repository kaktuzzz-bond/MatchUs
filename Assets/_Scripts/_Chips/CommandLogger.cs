using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CommandLogger
{
    private readonly Stack<ICommand> _stack = new();

    private readonly WaitForSeconds _wait = new(0.2f);


    public void AddCommand(ICommand command)
    {
        Debug.Log($"Adding {command}");

        _stack.Push(command);

        command.Execute();

        CheckStackCount();
    }


    public async UniTaskVoid UndoCommand()
    {
        if (_stack.Count == 0)
        {
            Debug.Log("Log is empty!");

            return;
        }

        ICommand command;

        List<UniTask> tasks = new();

        do
        {
            command = _stack.Pop();

            tasks.Add(command.Undo());
        } while (command.GetType() == typeof(RemoveSingleLineCommand));

        Debug.Log($"Undo ({tasks.Count}) commands");

        await UniTask.WhenAll(tasks);

        CheckStackCount();
    }


    public void CheckStackCount()
    {
        GameGUI.Instance.UndoButton.SetInteractivity(_stack.Count > 0);
    }
}