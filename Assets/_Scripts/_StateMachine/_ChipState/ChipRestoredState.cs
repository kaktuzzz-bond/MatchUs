using UnityEngine;
using DG.Tweening;

public class ChipRestoredState : IChip
{
    public override void Enter(ChipStateManager chip)
    {
        Vector3 targetPos = Board.Instance[chip.BoardPosition.x, chip.BoardPosition.y].position;

        Vector3 initPos = new(targetPos.x, targetPos.y + 0.5f, targetPos.z);

        chip.transform.position = initPos;

        chip.Renderer
                .DOFade(0, 0);

        chip.gameObject.SetActive(true);

        chip.transform
                .DOMoveY(targetPos.y, chip.fadeTime)
                .SetEase(Ease.Linear);
        
        // ???
        chip.SetFadedInState();
    }
}