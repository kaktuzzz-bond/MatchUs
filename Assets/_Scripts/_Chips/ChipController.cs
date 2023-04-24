using System;
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
    
    private void Awake()
    {
        _board = Board.Instance;
        _handler = ChipHandler.Instance;
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

        instance.name = $"Chip ({shapeIndex}, {colorIndex})";

        Chip chip = instance.GetComponent<Chip>();

        chip.Init(shapeIndex, colorIndex, boardPosition);

        OnChipCreated?.Invoke(chip);
    }
}