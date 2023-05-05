using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public class ChipRegistry : Singleton<ChipRegistry>
{
    [ShowInInspector, ReadOnly]
    public int Counter { get; private set; }

    [ShowInInspector]
    public List<Chip> InGameChips { get; private set; } = new();

    [ShowInInspector]
    public List<Chip> OutOfGameChips { get; private set; } = new();


    public void Register(Chip chip)
    {
        InGameChips.Add(chip);

        OutOfGameChips.Remove(chip);

        Counter++;
    }


    public void Unregister(Chip chip)
    {
        OutOfGameChips.Add(chip);

        InGameChips.Remove(chip);

        Counter--;
    }


    public void UnregisterAndDestroy(Chip chip)
    {
        InGameChips.Remove(chip);

        OutOfGameChips.Remove(chip);

        Counter--;
        
        Destroy(chip.gameObject);
    }


    public List<Chip> GetActiveChips()
    {
        return InGameChips
                .Where(c => c.ChipFiniteStateMachine.CurrentState.GetType() == typeof(FadedInChipState))
                .ToList();
    }
}