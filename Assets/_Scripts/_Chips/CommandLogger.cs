using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class CommandLogger
{
    public readonly Stack<ICommand> log;


    public CommandLogger()
    {
        log = new Stack<ICommand>();
    }


    public void Push(ICommand command)
    {
        log.Push(command);
    }


    public void Undo()
    {
        ICommand command = log.Pop();
        command.Undo();
    }
}