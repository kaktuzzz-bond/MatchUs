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

    private readonly WaitForSeconds _wait01 = new(0.02f);

    [SerializeField]
    private Chip _storage;

#region Component Links

    private Board _board;

    private ChipHandler _handler;

    private GameController _gameController;

    private CameraController _cameraController;

    private PointerController _pointerController;

#endregion


    private void Awake()
    {
        _board = Board.Instance;
        _handler = ChipHandler.Instance;
        _gameController = GameController.Instance;
        _cameraController = CameraController.Instance;
        _pointerController = PointerController.Instance;
    }


    private void CompareStorage(Chip chip)
    {
        
        
        bool isInPosition = chip.CompareHorizontalPosition(_storage) ||
                            chip.CompareVerticalPosition(_storage) ||
                            chip.CompareMultilinePosition(_storage);

        bool isComparing = chip.CompareShape(_storage) || 
                           chip.CompareColor(_storage);

        if (isInPosition && isComparing)
        {
            chip.StateManager.SetFadedOutState();

            _storage.StateManager.SetFadedOutState();

            _storage = null;

            _pointerController.HidePointers();
        }
        else
        {
            _pointerController.HidePointers();
            
            _pointerController.GetPointer(PointerController.Selector, chip.BoardPosition);
            
            _storage = chip;
        }
    }


    private void ProcessChip(Chip chip)
    {
        //case: Storage is empty
        if (_storage == null)
        {
            _pointerController.GetPointer(PointerController.Selector, chip.BoardPosition);

            _storage = chip;

            return;
        }

        //case: Tap the same
        if (chip.Equals(_storage))
        {
            Debug.LogWarning("The same chip");

            _storage = null;

            _pointerController.HidePointers();

            return;
        }

        // case: Compare chips
        CompareStorage(chip);
    }


    private void DrawStartArray()
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
        _gameController.OnGameStarted += DrawStartArray;
        _cameraController.OnChipTapped += ProcessChip;
    }


    private void OnDisable()
    {
        _gameController.OnGameStarted -= DrawStartArray;
        _cameraController.OnChipTapped -= ProcessChip;
    }

#endregion
}