using System;
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

    private Chip _chip;


    private void Awake()
    {
        _chip = GetComponent<Chip>();
    }


    private void Launch()
    {
        gameObject.SetActive(false);

        CurrentState = _enabledChipState;

        CurrentState.Enter(_chip);

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


    private void SetState(IChipState newState)
    {
        CurrentState = newState;
        CurrentState.Enter(_chip);
    }


#region Enable / Disable

    private void OnEnable()
    {
        _chip.OnInitialized += Launch;
    }


    private void OnDisable()
    {
        _chip.OnInitialized -= Launch;
    }

#endregion
}