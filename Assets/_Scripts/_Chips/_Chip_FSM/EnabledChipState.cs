using UnityEngine;

public class EnabledChipState : IChipState
{
    public void Enter(Chip chip)
    {
        Vector3 targetPos = Board.Instance[chip.BoardPosition.x, chip.BoardPosition.y].position;

        Vector3 initPos = new(targetPos.x, targetPos.y + 0.5f, targetPos.z);

        chip.transform.position = initPos;

        chip.Activate(true);

        ChipController.Instance.ChipRegistry.Register(chip);
        
        chip.VerticalShiftTo(targetPos.y);
    }
}