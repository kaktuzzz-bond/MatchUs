using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class ChipRegistry
{
    [ShowInInspector, ReadOnly]
    public int Counter => InGameChips.Count;

    [ShowInInspector]
    public List<Chip> InGameChips { get; private set; } = new();

    public List<Chip> ActiveChips => InGameChips
            .Where(c => c.CurrentState == Chip.States.LightOn)
            .OrderBy(c => c.BoardPosition.y)
            .ThenBy(c => c.BoardPosition.x)
            .ToList();

    private readonly List<Chip> _allChips = new();
    
    
    public void Register(Chip chip)
    {
        if (!_allChips.Contains(chip))
        {
            _allChips.Add(chip);
        }

        InGameChips.Add(chip);
    }
    
    public void Unregister(Chip chip)
    {
        InGameChips.Remove(chip);

        CheckCounter();
    }


    public void UnregisterAndDestroy(Chip chip)
    {
        _allChips.Remove(chip);

        InGameChips.Remove(chip);

        chip.Destroy();

        CheckCounter();
    }


    public List<Chip> GetChipsBelowLine(int boardLine)
    {
        return InGameChips.Where(chip => chip.BoardPosition.y > boardLine).ToList();
    }

    public List<Chip> GetChipsOnLineAndBelow(int boardLine)
    {
        return InGameChips.Where(chip => chip.BoardPosition.y >= boardLine).ToList();
    }
    
    
    public async UniTask ResetRegistry()
    {
        async UniTask ChipDestroy(Chip c)

        {
            c.Destroy();

            await UniTask.Yield();
        }

        var tasks = Enumerable
                .Select(_allChips, ChipDestroy)
                .ToList();

        await UniTask.WhenAll(tasks);

        InGameChips.Clear();

        _allChips.Clear();
    }


    private void CheckCounter()
    {
        if (Counter == 0)
        {
            GameManager.Instance.EndGame();
        }
    }

    
    public void CheckBoardCapacity()
    {
        int emptyCells = Board.Instance.Capacity - InGameChips.Count;

        GameGUI.Instance.AddButton.SetInteractivity(emptyCells >= Counter);
    }
}