using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChipHandler : Singleton<ChipHandler>
{
 

    private ChipController _chipController;

    private GameController _gameController;

    private Board _board;

 
    private readonly Stack<ICommand> _log = new();


    private void Awake()
    {
        _board = Board.Instance;

        _chipController = ChipController.Instance;

        _gameController = GameController.Instance;
    }


    public void Execute(ICommand command)
    {
        command.Execute();

        _log.Push(command);
    }


    public void Undo()
    {
        ICommand command = _log.Pop();
        command.Undo();
    }


  

    
 
}