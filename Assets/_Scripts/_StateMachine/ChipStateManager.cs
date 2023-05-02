using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Chip))]
public class ChipStateManager : MonoBehaviour
{
    [ShowInInspector]
    public IChipState CurrentState { get; private set; } = new EnabledChipState();

    private readonly EnabledChipState _enabledChipState = new();
    
    private readonly FadedOutChipState _fadedOutChipState = new();
    
    private readonly FadedInChipState _fadedInChipState = new();
    
    private readonly DisabledChipState _disabledChipState = new();
    
    private readonly SelfDestroyableChipState _selfDestroyableChipState = new();

    public Chip Chip { get; private set; }


    private void Awake()
    {
        Chip = GetComponent<Chip>();
    }


    private void Launch()
    {
        gameObject.SetActive(false);

        CurrentState = _enabledChipState;

        CurrentState.Enter(Chip);

        SetFadedInState();
    }


    [Button("Set Enabled State")]
    public void SetEnabledState()
    {
        SetState(_enabledChipState);
    }


    [Button("Set Faded Out State")]
    public void SetFadedOutState()
    {
        SetState(_fadedOutChipState);
    }


    [Button("Set Faded In State")]
    public void SetFadedInState()
    {
        SetState(_fadedInChipState);
    }


    [Button("Set Disabled State")]
    public void SetDisabledState()
    {
        SetState(_disabledChipState);
    }

    [Button("Set SelfDestroyable State")]
    public void SetSelfDestroyableState()
    {
        SetState(_selfDestroyableChipState);
    }
    
    private void SetState(IChipState newState)
    {
        CurrentState = newState;
        CurrentState.Enter(Chip);
    }


    public static void DisableChips(List<ChipStateManager> states)
    {
        foreach (ChipStateManager state in states)
        {
            state.SetDisabledState();
        }
    }
    
#region Enable / Disable

    private void OnEnable()
    {
        Chip.OnInitialized += Launch;
    }


    private void OnDisable()
    {
        Chip.OnInitialized -= Launch;
    }

#endregion
}