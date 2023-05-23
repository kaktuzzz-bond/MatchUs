using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public static class ChipRegistry
{
    [ShowInInspector, ReadOnly]
    public static int Counter => InGameChips.Count;

    [ShowInInspector]
    public static List<Chip> InGameChips { get; private set; } = new();

    public static List<Chip> ActiveChips => InGameChips
            .Where(c => c.CurrentState == Chip.States.LightOn)
            .OrderBy(c => c.BoardPosition.y)
            .ThenBy(c => c.BoardPosition.x)
            .ToList();

    private static readonly List<Chip> AllChips = new();
    
    
    public static void Register(Chip chip)
    {
        if (!AllChips.Contains(chip))
        {
            AllChips.Add(chip);
        }

        InGameChips.Add(chip);
    }
    
    public static void Unregister(Chip chip)
    {
        InGameChips.Remove(chip);

        CheckCounter();
    }


    public static async UniTaskVoid UnregisterAndDestroy(Chip chip)
    {
        AllChips.Remove(chip);

        InGameChips.Remove(chip);

        await chip.RemoveFromBoardAsync();
        
        chip.Destroy();

        CheckCounter();
    }


    public static List<Chip> GetChipsBelowLine(int boardLine)
    {
        return InGameChips.Where(chip => chip.BoardPosition.y > boardLine).ToList();
    }

    public static List<Chip> GetChipsOnLineAndBelow(int boardLine)
    {
        return InGameChips.Where(chip => chip.BoardPosition.y >= boardLine).ToList();
    }
    
    
    public static async UniTask ResetRegistry()
    {
        async UniTask ChipDestroy(Chip c)

        {
            c.Destroy();

            await UniTask.Yield();
        }

        var tasks = Enumerable
                .Select(AllChips, ChipDestroy)
                .ToList();

        await UniTask.WhenAll(tasks);

        InGameChips.Clear();

        AllChips.Clear();
    }


    private static void CheckCounter()
    {
        if (Counter == 0)
        {
            GameManager.Instance.EndGame();
        }
    }

    
    public static void CheckBoardCapacity()
    {
        int emptyCells = Board.Instance.Capacity - InGameChips.Count;

        GameGUI.Instance.AddButton.SetInteractivity(emptyCells >= Counter);
    }
}