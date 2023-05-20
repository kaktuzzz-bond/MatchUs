using System;
using UnityEngine;

public class GameStorage : MonoBehaviour
{
    public GameBoard Board { get; private set; }


    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }


    public void SetGameBoard()
    {
        Board = new GameBoard(_gameManager.gameData.width, _gameManager.gameData.height);
    }


    public void ResetGameBoard()
    {
        Board = null;
    }
}