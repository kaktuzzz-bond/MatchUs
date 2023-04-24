using UnityEngine;
using DG.Tweening;

public class ChipEnabledState : IChip
{
    public void Enter(Chip chip)
    {
        Vector3 targetPos = Board.Instance[chip.BoardPosition.x, chip.BoardPosition.y].position;

        Vector3 initPos = new(targetPos.x, targetPos.y + 0.5f, targetPos.z);

        chip.transform.position = initPos;

        chip.Renderer
                .DOFade(0, 0);

        chip.gameObject.SetActive(true);

        chip.transform
                .DOMoveY(targetPos.y, Chip.FadeTime)
                .SetEase(Ease.Linear);
    }
}