using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Chip))]
public class ChipStateManager : MonoBehaviour
{
    [ShowInInspector]
    private IChipState _currentState = new EnabledChipState();

    //private readonly InitializedChipState initializedChipState = new();
    
    private readonly EnabledChipState enabledChipState = new();

    private readonly FadedOutChipState fadedOutChipState = new();

    private readonly FadedInChipState fadedInChipState = new();

    private readonly DisabledChipState disabledChipState = new();

    private Chip _chip;


    private void Awake()
    {
        _chip = GetComponent<Chip>();
    }


    private void Launch()
    {
        gameObject.SetActive(false);
        
        _currentState = enabledChipState;

        _currentState.Enter(_chip);
        
        SetFadedInState();
    }


    [Button("Set Enabled State")]
    public void SetEnabledState() => SetState(enabledChipState);


    [Button("Set Faded Out State")]
    public void SetFadedOutState() => SetState(fadedOutChipState);


    [Button("Set Faded In State")]
    public void SetFadedInState() => SetState(fadedInChipState);


    [Button("Set Disabled State")]
    public void SetDisabledState() => SetState(disabledChipState);


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