using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public class ChipRegistry
{
    [ShowInInspector, ReadOnly]
    public int Counter => InGameChips.Count;

    [ShowInInspector]
    public List<Chip> InGameChips { get; private set; } = new();

    public List<Chip> ActiveChips => InGameChips
            .Where(c => c.ChipFiniteStateMachine.CurrentState.GetType() == typeof(FadedInChipState))
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

        //CameraController.Instance.MoveToBottomBound();
    }


    public void Unregister(Chip chip)
    {
        InGameChips.Remove(chip);

        //CameraController.Instance.MoveToBottomBound();

        CheckCounter();
    }


    public void UnregisterAndDestroy(Chip chip)
    {
        _allChips.Remove(chip);

        InGameChips.Remove(chip);

        chip.Destroy();

        //CameraController.Instance.MoveToBottomBound();

        CheckCounter();
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
        int emptyCells = Board.Instance.BoardCapacity - InGameChips.Count;

        GameGUI.Instance.AddButton.SetInteractivity(emptyCells >= Counter);
    }
}