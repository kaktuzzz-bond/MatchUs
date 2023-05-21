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
        switch (newState)
        {
            case Chip.States.LightOn:

                CurrentState = _lightOn;

                break;

            case Chip.States.LightOff:

                CurrentState = _lightOff;

                break;

            case Chip.States.Removed:

                CurrentState = _removed;

                break;

            default:

                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        CurrentState.Enter(_chip);
    }
}