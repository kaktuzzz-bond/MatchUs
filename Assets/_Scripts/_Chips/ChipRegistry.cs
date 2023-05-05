using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public class ChipRegistry : Singleton<ChipRegistry>
{
    [ShowInInspector, ReadOnly]
    public int Counter => InGameChips.Count;

    [ShowInInspector]
    public List<Chip> InGameChips { get; private set; } = new();

    [ShowInInspector]
    public HashSet<Chip> AllChips { get; private set; } = new();
    
    [ShowInInspector]
    public List<Chip> OutOfGameChips { get; private set; } = new();

    public List<Chip> ActiveChips => AllChips
            .Where(c => c.ChipFiniteStateMachine.CurrentState.GetType() == typeof(FadedInChipState))
            .ToList();


    public void Register(Chip chip)
    {
        AllChips.Add(chip);
        
        InGameChips.Add(chip);

        OutOfGameChips.Remove(chip);
    }


    public void Unregister(Chip chip)
    {
        OutOfGameChips.Add(chip);
    
        InGameChips.Remove(chip);
    }


    public void UnregisterAndDestroy(Chip chip)
    {
        AllChips.Remove(chip);
        
        InGameChips.Remove(chip);

        OutOfGameChips.Remove(chip);

        Destroy(chip.gameObject);
    }
}