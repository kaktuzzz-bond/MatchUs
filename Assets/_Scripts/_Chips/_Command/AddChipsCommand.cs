using System.Collections.Generic;
using System.Linq;

public class AddChipsCommand : ICommand
{
    private readonly ChipController _chipController;

    private readonly ChipRegistry _chipRegistry;

    private List<Chip> _addedChips;


    public AddChipsCommand()
    {
        _chipController = ChipController.Instance;
        _chipRegistry = ChipRegistry.Instance;
    }


    public void Execute()
    {
        var inGameChips = _chipRegistry.GetActiveChips();

        _chipController.CloneInGameChips(inGameChips, out _addedChips);

        _chipController.Log.AddCommand(this);
    }


    public void Undo()
    {
        foreach (Chip chip in _addedChips)
        {
            chip.ChipFiniteStateMachine.SetSelfDestroyableState();
        }

        _addedChips.Clear();
    }
}