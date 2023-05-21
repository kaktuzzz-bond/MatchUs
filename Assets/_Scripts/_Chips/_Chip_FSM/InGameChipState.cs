using Cysharp.Threading.Tasks;
using UnityEngine;

public class InGameChipState : IChipState
{
    public async void Enter(Chip chip)
    {
        Vector3 targetPos = Board.Instance[chip.BoardPosition.x, chip.BoardPosition.y];

        Vector3 initPos = new(targetPos.x, targetPos.y + 0.5f, targetPos.z);

        chip.transform.position = initPos;

        ChipController.Instance.ChipRegistry.Register(chip);

        chip.Activate(true);

        chip.VerticalShiftAsync(targetPos.y).Forget();
        
        await UniTask.Yield();
    }
}