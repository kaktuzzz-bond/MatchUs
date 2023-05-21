using Cysharp.Threading.Tasks;
using UnityEngine;

public class RemovedChipState : IChipState
{
    public void Enter(Chip chip)
    {
        LightOutAsync(chip).Forget();
    }


    private async UniTaskVoid LightOutAsync(Chip chip)
    {
        await UniTask.WhenAll(
                chip.RemoveFromBoardAsync(),
                chip.Fade(0f));

        ChipController.Instance.ChipRegistry.Unregister(chip);

        chip.Activate(false);
    }
}