using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

[RequireComponent(typeof(ChipRegistry))]
public class ChipController : Singleton<ChipController>
{
    [SerializeField]
    private Transform chipPrefab;

    private Vector2Int NextBoardPosition =>
            new(_chipRegistry.Counter % _board.Width, _chipRegistry.Counter / _board.Width);

    private readonly WaitForSeconds _wait01 = new(0.01f);

    public CommandLogger Log { get; } = new();

#region COMPONENTS LINKS

    private Board _board;

    private GameController _gameController;

    private CameraController _cameraController;

    private ChipComparer _chipComparer;

    private ChipRegistry _chipRegistry;

#endregion


    private void Awake()
    {
        _board = Board.Instance;
        _gameController = GameController.Instance;
        _chipComparer = ChipComparer.Instance;
        _cameraController = CameraController.Instance;

        _chipRegistry = ChipRegistry.Instance;
    }


    public void AddChips()
    {
        ICommand command = new AddChipsCommand();

        command.Execute();
    }


    private void ProcessMatched(Chip first, Chip second)
    {
        ICommand command = new FadeOutCommand(first, second);

        command.Execute();

        int firstLine = first.BoardPosition.y;

        int secondLine = second.BoardPosition.y;

        if (firstLine == secondLine)
        {
            _board.CheckLine(firstLine);

            return;
        }

        int topLine = Mathf.Min(first.BoardPosition.y, second.BoardPosition.y);

        int bottomLine = Mathf.Max(first.BoardPosition.y, second.BoardPosition.y);

        _board.CheckLine(bottomLine);

        _board.CheckLine(topLine);
    }


#region PLACE ON BOARD

    private void DrawStartArray()
    {
        StartCoroutine(DrawStartChipsRoutine(_gameController.ChipsOnStartNumber));
    }


    private IEnumerator DrawStartChipsRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _ = CreateChip();

            yield return _wait01;
        }
    }


    private readonly List<Chip> _addedChips = new();


    public void CloneInGameChips(List<Chip> chips, out List<Chip> addedChips)
    {
        StartCoroutine(CloneInGameChipsRoutine(chips));

        addedChips = _addedChips;
    }


    private IEnumerator CloneInGameChipsRoutine(List<Chip> chips)
    {
        foreach (Chip newChip in chips.Select(chip => CreateChip(chip.ShapeIndex, chip.ColorIndex)))
        {
            _addedChips.Add(newChip);

            yield return _wait01;
        }
    }

#endregion

#region CHIP CREATION

    private Chip CreateChip(int shapeIndex, int colorIndex)
    {
        Vector2Int boardPos = NextBoardPosition;

        return CreateNewChip(shapeIndex, colorIndex, boardPos);
    }


    private Chip CreateChip()
    {
        ChipData data = GetChipDataByChance();

        Vector2Int boardPos = NextBoardPosition;

        return CreateNewChip(data.shapeIndex, data.colorIndex, boardPos);
    }


    private Chip CreateNewChip(int shapeIndex, int colorIndex, Vector2Int boardPosition)
    {
        Vector3 worldPos = _board[boardPosition.x, boardPosition.y].position;

        Transform instance = Instantiate(chipPrefab, worldPos, Quaternion.identity, _board.chipParent);

        Chip chip = instance.GetComponent<Chip>();

        chip.Init(shapeIndex, colorIndex, boardPosition);

        instance.name = $"Chip ({shapeIndex}, {colorIndex})";

        return chip;
    }

#endregion

#region CHIP DATA

    private ChipData GetChipDataByChance()
    {
        return Random.value <= _gameController.ChanceForRandom
                ? GetRandomChipData()
                : GetChipDataForHardLevel();
    }


    private ChipData GetRandomChipData()
    {
        int shapeIndex = Random.Range(0, _board.ShapePalletLength);

        int colorIndex = Random.Range(0, _board.ColorPalletLength);

        return new ChipData(shapeIndex, colorIndex);
    }


    private ChipData GetChipDataForHardLevel()
    {
        List<int> shapeIndexes = new(_board.ShapeIndexes);
        List<int> colorIndexes = new(_board.ColorIndexes);

        if (_chipRegistry.Counter > 0)
        {
            shapeIndexes.Remove(_chipRegistry.InGameChips.Last().ShapeIndex);
            colorIndexes.Remove(_chipRegistry.InGameChips.Last().ColorIndex);

            if (_chipRegistry.Counter >= _board.Width)
            {
                Chip chip = _chipRegistry.InGameChips[^_board.Width];

                shapeIndexes.Remove(chip.ShapeIndex);
                colorIndexes.Remove(chip.ColorIndex);
            }
        }

        int shapeIndex = shapeIndexes[Random.Range(0, shapeIndexes.Count)];
        int colorIndex = colorIndexes[Random.Range(0, colorIndexes.Count)];

        return new ChipData(shapeIndex, colorIndex);
    }

#endregion

#region ENABLE / DISABLE

    private void OnEnable()
    {
        _cameraController.OnCameraSetup += DrawStartArray;
        _chipComparer.OnChipMatched += ProcessMatched;
    }


    private void OnDisable()
    {
        _cameraController.OnCameraSetup -= DrawStartArray;
        _chipComparer.OnChipMatched -= ProcessMatched;
    }

#endregion
}