using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Chip))]
public class ChipFiniteStateMachine : MonoBehaviour
{
    [ShowInInspector]
    public IChipState CurrentState { get; private set; }
    
    public Chip Chip { get; private set; }


    private void Awake()
    {
        Chip = GetComponent<Chip>();
    }


    //[Button("Launch")]
    private void Launch()
    {
        gameObject.SetActive(false);

        CurrentState = new EnabledChipState();

        CurrentState.Enter(Chip);

        SetFadedInState();
    }

   private void SetEnabledState()
    {
        SetState(new EnabledChipState());
    }

    public void SetFadedOutState()
    {
        SetState(new FadedOutChipState());
    }

    public void SetFadedInState()
    {
        SetState(new FadedInChipState());
    }

    public void SetSelfDestroyableState()
    {
        SetState(new SelfDestroyableChipState());
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
        SetState(new DisabledChipState());
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