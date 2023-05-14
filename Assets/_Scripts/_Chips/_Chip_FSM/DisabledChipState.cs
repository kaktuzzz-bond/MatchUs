using UnityEngine;

public class DisabledChipState : IChipState
{
    public void Enter(Chip chip)
    {
        Vector3 initPos = chip.transform.position;

        Vector3 targetPos = new(initPos.x, initPos.y + 0.5f, initPos.z);

        chip.Fade(0f);

        chip.VerticalShiftTo(
                targetPos.y,
                () =>
                {
                    chip.Activate(false);
                    ChipController.Instance.ChipRegistry.Unregister(chip);
                });
    }
}