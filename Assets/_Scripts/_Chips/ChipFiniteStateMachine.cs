#define ENABLE_LOGS
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


    [Button("Launch")]
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
        Logger.Debug("STATE: Enabled");
        SetState(_enabledChipState);
    }

    [Button("Set Faded Out State")]
    public void SetFadedOutState()
    {
        Logger.Debug("STATE: Fade Out");
        SetState(_fadedOutChipState);
    }

    [Button("Set Faded In State")]
    public void SetFadedInState()
    {
        Logger.Debug("STATE: Fade In");
        SetState(_fadedInChipState);
    }

    [Button("Set Self-Destroy State")]
    public void SetSelfDestroyableState()
    {
        Logger.Debug("STATE: Destroy");
        SetState(_selfDestroyableChipState);
    }


    public static void DisableChips(List<ChipFiniteStateMachine> states)
    {
        foreach (ChipFiniteStateMachine state in states)
        {
            state.SetDisabledState();
        }
    }
    

    [Button("Set Disabled State")]
    private void SetDisabledState()
    {
        Logger.Debug("STATE: Disabled");
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