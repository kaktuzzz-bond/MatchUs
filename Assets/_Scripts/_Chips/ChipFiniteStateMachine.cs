using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Chip))]
public class ChipFiniteStateMachine : MonoBehaviour
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


    //[Button("Launch")]
    private void Launch()
    {
        gameObject.SetActive(false);

        CurrentState = _enabledChipState;

        CurrentState.Enter(Chip);

        SetFadedInState();
    }

   private void SetEnabledState()
    {
        SetState(_enabledChipState);
    }

    public void SetFadedOutState()
    {
        SetState(_fadedOutChipState);
    }

    public void SetFadedInState()
    {
        SetState(_fadedInChipState);
    }

    public void SetSelfDestroyableState()
    {
        SetState(_selfDestroyableChipState);
    }

    public void SetRestoredState()
    {
        SetFadedOutState();

        SetEnabledState();
    }

    public static void DisableChips(List<ChipFiniteStateMachine> states)
    {
        foreach (ChipFiniteStateMachine state in states)
        {
            state.SetDisabledState();
        }
    }
    
    private void SetDisabledState()
    {
        SetState(_disabledChipState);
    }


    private void SetState(IChipState newState)
    {
        CurrentState = newState;
        CurrentState.Enter(Chip);
    }


#region ENABLE / DISABLE

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