using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Chip))]
public class ChipStateManager : MonoBehaviour
{
    [ShowInInspector]
    private IChipState _currentState = new ChipStateEnabledState();

    private readonly ChipStateEnabledState stateEnabledState = new();

    private readonly ChipStateFadedOutState stateFadedOutState = new();

    private readonly ChipStateFadedInState stateFadedInState = new();

    private readonly ChipStateDisabledState stateDisabledState = new();

    private Chip _chip;


    private void Awake()
    {
        _chip = GetComponent<Chip>();
    }


    private void Launch()
    {
        gameObject.SetActive(false);
        
        _currentState = stateEnabledState;

        _currentState.Enter(_chip);
        
        SetFadedInState();
    }


    [Button("Set Enabled State")]
    public void SetEnabledState() => SetState(stateEnabledState);


    [Button("Set Faded Out State")]
    public void SetFadedOutState() => SetState(stateFadedOutState);


    [Button("Set Faded In State")]
    public void SetFadedInState() => SetState(stateFadedInState);


    [Button("Set Disabled State")]
    public void SetDisabledState() => SetState(stateDisabledState);


    private void SetState(IChipState newState)
    {
        _currentState = newState;
        _currentState.Enter(_chip);
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