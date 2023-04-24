using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChipController : Singleton<ChipController>
{
    public event Action<Chip> OnChipCreated;

    [SerializeField]
    private Transform chipPrefab;

    private Board _board;

    private ChipHandler _handler;

    private GameController _gameController;

    private readonly WaitForSeconds _wait01 = new(0.05f);


    private void Awake()
    {
        _board = Board.Instance;
        _handler = ChipHandler.Instance;
        _gameController = GameController.Instance;
    }


    private void DrawStartChips()
    {
        StartCoroutine(DrawStartChipsRoutine());
    }


    private IEnumerator DrawStartChipsRoutine()
    {
        for (int i = 0; i < (int)_gameController.DifficultyLevel; i++)
        {
            Create();

            yield return _wait01;
        }
    }


    [Button("Create Chip")]
    private void Create()
    {
        ChipData data = _handler.GetNewChipData();

        Vector2Int boardPos = _handler.NextBoardPosition;

        CreateNewChip(data.shapeIndex, data.colorIndex, boardPos);
    }


    private void CreateNewChip(int shapeIndex, int colorIndex, Vector2Int boardPosition)
    {
        Vector3 worldPos = _board[boardPosition.x, boardPosition.y].position;

        Transform instance = Instantiate(chipPrefab, worldPos, Quaternion.identity, _board.chipParent);
       
        Chip chip = instance.GetComponent<Chip>();
        
        chip.Init(shapeIndex, colorIndex, boardPosition);

        instance.name = $"Chip ({shapeIndex}, {colorIndex})";

        OnChipCreated?.Invoke(chip);
    }


#region Enable / Disable

    private void OnEnable()
    {
        _gameController.OnGameStarted += DrawStartChips;
    }


    private void OnDisable()
    {
        _gameController.OnGameStarted -= DrawStartChips;
    }

#endregion
}