using DG.Tweening;
using UnityEngine;

public class SelfDestroyableChipState : IChipState
{
    public void Enter(Chip chip)
    {
        Vector3 initPos = chip.transform.position;

        Vector3 targetPos = new(initPos.x, initPos.y + 0.5f, initPos.z);

        chip.Fade(0f);

        chip.VerticalShiftTo(
                targetPos.y,
                () =>  ChipController.Instance.ChipRegistry.UnregisterAndDestroy(chip));
    }
}