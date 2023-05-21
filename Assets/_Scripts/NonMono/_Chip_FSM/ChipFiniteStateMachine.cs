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
        
        //SetState(chip.CurrentState);
    }


    public void LightOn()
    {
        SetState(_lightOn);
    }


    public void LightOff()
    {
        SetState(_lightOff);
    }


    public void Removed()
    {
        SetState(_removed);
    }
    private void SetState(IChipState newState)
    {
        CurrentState = newState;

        CurrentState.Enter(_chip);
    }
}