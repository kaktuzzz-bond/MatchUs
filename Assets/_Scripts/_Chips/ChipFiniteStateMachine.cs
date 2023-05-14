using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
    public async UniTask Launch()
    {
        gameObject.SetActive(false);

        CurrentState = new EnabledChipState();

        CurrentState.Enter(Chip);

        await SetFadedInState();
    }


    private async UniTask SetEnabledState()
    {
        await SetState(new EnabledChipState());
    }


    public async UniTask SetFadedOutState()
    {
        await SetState(new FadedOutChipState());
    }


    public async UniTask SetFadedInState()
    {
        await SetState(new FadedInChipState());
    }


    public async UniTask SetSelfDestroyableState()
    {
        await SetState(new SelfDestroyableChipState());
    }


    public async UniTask SetRestoredState()
    {
        await SetFadedOutState();

        await SetEnabledState();
    }


    public static void DisableChips(List<ChipFiniteStateMachine> states)
    {
        foreach (ChipFiniteStateMachine state in states)
        {
            state.SetDisabledState().Forget();
        }
    }


    private async UniTask SetDisabledState()
    {
        await SetState(new DisabledChipState());
    }


    private async UniTask SetState(IChipState newState)
    {
        CurrentState = newState;
        await CurrentState.Enter(Chip);
    }
}