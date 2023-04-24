using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChipController : Singleton<ChipController>
{
    public event Action<Chip> OnChipCreated;

    [SerializeField]
    private Transform chipPrefab;

    private readonly WaitForSeconds _wait01 = new(0.05f);

    [ShowInInspector]
    private Chip _storage;

#region Component Links

    private Board _board;

    private ChipHandler _handler;

    private GameController _gameController;

    private CameraController _cameraController;

#endregion


    [Button("Clear Storage")]
    private void ClearStorage()
    {
        _storage = null;
    }


    private void Awake()
    {
        _board = Board.Instance;
        _handler = ChipHandler.Instance;
        _gameController = GameController.Instance;
        _cameraController = CameraController.Instance;
    }


    private void CompareStorage(Chip chip)
    {
        Debug.Log($"Shape: {chip.CompareShape(_storage)}");

        Debug.Log($"Color: {chip.CompareColor(_storage)}");

        Debug.Log($"Vertical: {chip.CompareVerticalPosition(_storage)}");

        Debug.Log($"Horizontal: {chip.CompareHorizontalPosition(_storage)}");

        Debug.Log($"Multiline: {chip.CompareMultilinePosition(_storage)}");
    }


    private void ProcessChip(Chip chip)
    {
        if (chip.ChipState.GetType() == typeof(FadedInChipState))
        {
            if (_storage == null)
            {
                _storage = chip;
            }
            else
            {
                CompareStorage(chip);
            }

            Debug.LogWarning(
                    $"Data ({chip.ShapeIndex}, {chip.ColorIndex}); " +
                    $"Pos {chip.BoardPosition}");

            chip.Shake();
        }
        else
        {
            Debug.Log("Not active chip");
        }
    }


    private void DrawStartChips()
    {
        StartCoroutine(DrawStartChipsRoutine());
    }


    private IEnumerator DrawStartChipsRoutine()
    {
        for (int i = 0; i < (int)_gameController.DifficultyLevel; i++)
        {
            CreateChip();

            yield return _wait01;
        }
    }


    private void CreateChip()
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
        _cameraController.OnChipTapped += ProcessChip;
    }


    private void OnDisable()
    {
        _gameController.OnGameStarted -= DrawStartChips;
        _cameraController.OnChipTapped -= ProcessChip;
    }

#endregion
}