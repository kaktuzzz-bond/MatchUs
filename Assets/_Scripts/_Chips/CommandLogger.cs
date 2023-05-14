using System;
using System.Collections;
using System.Collections.Generic;
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
    

    public IEnumerator UndoCommand()
    {
        if (_stack.Count == 0)
        {
            Debug.Log("Log is empty!");
            
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
        
        CheckStackCount();
    }


    public void CheckStackCount()
    {
        GameGUI.Instance.UndoButton.SetInteractivity(_stack.Count > 0);
    }
}