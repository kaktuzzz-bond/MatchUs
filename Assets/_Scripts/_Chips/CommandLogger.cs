using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CommandLogger
{
    private readonly Stack<ICommand> _stack = new();

    private readonly WaitForSeconds _wait = new(0.2f);


    public void AddCommand(ICommand command)
    {
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

        do
        {
            command = _stack.Pop();

            await command.Undo();

            await UniTask.WaitForFixedUpdate();
        } while (command.GetType() == typeof(RemoveSingleLineCommand));

        CheckStackCount();
    }


    public void CheckStackCount()
    {
        GameGUI.Instance.UndoButton.SetInteractivity(_stack.Count > 0);
    }
}