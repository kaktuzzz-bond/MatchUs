using Cysharp.Threading.Tasks;
using UnityEngine;

public class DestroySelfChipState : IChipState
{
    public async void Enter(Chip chip)
    {
        Vector3 initPos = chip.transform.position;

        Vector3 targetPos = new(initPos.x, initPos.y + 0.5f, initPos.z);

        chip.Fade(0f);

        await chip.VerticalShiftAsync(targetPos.y);

        ChipController.Instance.ChipRegistry.UnregisterAndDestroy(chip);
    }
}