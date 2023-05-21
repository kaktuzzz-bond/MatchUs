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
        Vector3 initPos = chip.transform.position;

        Vector3 targetPos = new(initPos.x, initPos.y + 0.5f, initPos.z);

        await chip.VerticalShiftAsync(targetPos.y);

        await chip.Fade(0f);

        chip.Activate(false);

        ChipController.Instance.ChipRegistry.Unregister(chip);
    }
}