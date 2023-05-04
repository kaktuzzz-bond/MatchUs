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


    public void SetEnabledState()
    {
        Debug.Log("STATE: Enabled");
        SetState(_enabledChipState);
    }


    public void SetFadedOutState()
    {
        Debug.Log("STATE: Fade Out");
        SetState(_fadedOutChipState);
    }


    public void SetFadedInState()
    {
        Debug.Log("STATE: Fade In");
        SetState(_fadedInChipState);
    }


    public void SetSelfDestroyableState()
    {
        Debug.Log("STATE: SelfDestroy");
        SetState(_selfDestroyableChipState);
    }


    public static void DisableChips(List<ChipStateManager> states)
    {
        foreach (ChipStateManager state in states)
        {
            state.SetDisabledState();
        }
    }
    


    private void SetDisabledState()
    {
        Debug.Log("STATE: Disabled");
        SetState(_disabledChipState);
    }


    private void SetState(IChipState newState)
    {
        CurrentState = newState;
        CurrentState.Enter(Chip);
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