using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class ChipController : Singleton<ChipController>
{
    public ChipRegistry Registry { get; private set; }

    private CommandLogger _commandLogger;

    private GameManager _gameManager;


    private void Awake()
    {
        Registry = new ChipRegistry();

        _commandLogger = new CommandLogger();

        _gameManager = GameManager.Instance;
    }


    public async UniTask AddChips()
    {
        //PointerController.HidePointers();

        var infos = ChipInfo.ExtractInfos(Registry.ActiveChips);

        var cloned = ChipInfo.GetClonedInfo(infos, Registry.Counter);

        await DrawArrayAsync(cloned);

        ChipComparer.ClearStorage();

        _commandLogger.AddCommand(new AddChipsCommand());
    }


    public void ShuffleChips()
    {
        //PointerController.HidePointers();

        ChipComparer.ClearStorage();

        _commandLogger.AddCommand(new ShuffleCommand());
    }


    public void ShowHints()
    {
        //PointerController.ShowHints();
    }


    public void UndoCommand()
    {
        _commandLogger.UndoCommand().Forget();
    }


    public void ProcessTappedChip(Chip chip)
    {
        var tapped = ChipComparer.HandleTap(chip);

        if (tapped == null)
        {
            Board.Instance.HideSelector();

            return;
        }

        if (tapped.Length == 1)
        {
            Board.Instance.ShowSelector(chip.transform.position);

            return;
        }

        Matching(tapped[0], tapped[1]);
    }


    private void Matching(Chip first, Chip second)
    {
        Board.Instance.HideSelector();

        _commandLogger.AddCommand(new FadeOutCommand(first, second));

        var emptyLines = LineChecker.GetEmptyLines(first, second);

        if (emptyLines.Count == 0) return;

        RemoveLines(emptyLines).Forget();
    }


    private async UniTaskVoid RemoveLines(List<List<Chip>> lines)
    {
        foreach (var list in lines)
        {
            int lineNumber = list.First().BoardPosition.y;

            _commandLogger.AddCommand(new RemoveSingleLineCommand(list, lineNumber, Registry));

            await MoveChipsUpAsync(lineNumber);

            CameraController.Instance.MoveToBottomBound();

            Registry.CheckBoardCapacity();
        }
    }


    private async UniTask MoveChipsUpAsync(int lineNumber)
    {
        var chipsBelow = Registry.GetChipsBelowLine(lineNumber);

        List<UniTask> tasks = new();

        foreach (Chip chip in chipsBelow)
        {
            tasks.Add(chip.MoveUpAsync());
        }

        await UniTask.WhenAll(tasks);
    }


    
    
    
    public async UniTask DrawArrayAsync(List<ChipInfo> chipInfos)
    {
        int line = (int)chipInfos.First().position.y;

        foreach (ChipInfo info in chipInfos)
        {
            if ((int)info.position.y != line)
            {
                line = (int)info.position.y;

                await UniTask.Delay(100);
            }

            Chip chip = CreateChip(info);

            Registry.Register(chip);

            chip.Init(info);

            chip.PlaceOnBoardAsync().Forget();
        }

        _commandLogger.CheckStackCount();
    }


    private Chip CreateChip(ChipInfo info)
    {
        Transform instance = Instantiate(
                _gameManager.gameData.chipPrefab,
                info.position,
                Quaternion.identity,
                _gameManager.gameData.chipParent);

        if (!instance.TryGetComponent(out Chip chip)) return null;

        instance.name = $"Chip ({info.shapeIndex}, {info.colorIndex})";

        return chip;
    }
}