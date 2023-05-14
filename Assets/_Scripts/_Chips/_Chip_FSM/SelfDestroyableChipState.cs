using Cysharp.Threading.Tasks;
using UnityEngine;

public class SelfDestroyableChipState : IChipState
{
    public void Enter(Chip chip)
    {
        Vector3 initPos = chip.transform.position;

        Vector3 targetPos = new(initPos.x, initPos.y + 0.5f, initPos.z);

        chip.Fade(0f);

        VerticalShift(chip, targetPos.y).Forget();
    }


    private async UniTaskVoid VerticalShift(Chip chip, float targetY)
    {
        await chip.VerticalShiftTo(targetY);

        ChipController.Instance.ChipRegistry.UnregisterAndDestroy(chip);
    }
}