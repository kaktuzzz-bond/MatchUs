using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class ChipFiniteStateMachine
{
    public IChipState CurrentState { get; private set; }

    private readonly LightedOnChipState _lightOn = new();

    private readonly LightedOffChipState _lightOff = new();

    private readonly RemovedChipState _removed = new();

    private readonly Chip _chip;

    public ChipFiniteStateMachine(Chip chip)
    {
        _chip = chip;
        
        CurrentState = new RemovedChipState();
        
        SetState(chip.CurrentState);
    }
   

    public void SetState(Chip.States newState)
    {
        CurrentState = newState switch
        {
                Chip.States.LightOn => _lightOn,
                Chip.States.LightOff => _lightOff,
                Chip.States.Removed => _removed,
                _ => throw new ArgumentOutOfRangeException(nameof(newState), newState, null)
        };

        CurrentState.Enter(_chip);
    }
}