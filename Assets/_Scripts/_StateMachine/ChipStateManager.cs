using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Chip))]
public class ChipStateManager : MonoBehaviour
{
    [ShowInInspector]
    private IChip _currentState = new ChipEnabledState();

    private readonly ChipEnabledState enabledState = new();

    private readonly ChipFadedOutState fadedOutState = new();

    private readonly ChipFadedInState fadedInState = new();

    private readonly ChipDisabledState disabledState = new();

    private Chip _chip;


    private void Awake()
    {
        _chip = GetComponent<Chip>();
    }


    private void Launch()
    {
        gameObject.SetActive(false);
        
        _currentState = enabledState;

        _currentState.Enter(_chip);
        
        SetFadedInState();
    }


    [Button("Set Enabled State")]
    public void SetEnabledState() => SetState(enabledState);


    [Button("Set Faded Out State")]
    public void SetFadedOutState() => SetState(fadedOutState);


    [Button("Set Faded In State")]
    public void SetFadedInState() => SetState(fadedInState);


    [Button("Set Disabled State")]
    public void SetDisabledState() => SetState(disabledState);


    private void SetState(IChip newState)
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