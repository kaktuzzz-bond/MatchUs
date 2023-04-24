using UnityEngine;
using DG.Tweening;

public class EnabledChipState : IChipState
{
    public void Enter(Chip chip)
    {
        //Debug.Log("Enabled state Enter");
        
        Vector3 targetPos = Board.Instance[chip.BoardPosition.x, chip.BoardPosition.y].position;

        Vector3 initPos = new(targetPos.x, targetPos.y + 0.5f, targetPos.z);

        chip.transform.position = initPos;

        chip.gameObject.SetActive(true);

        chip.transform
                .DOMoveY(targetPos.y, Chip.FadeTime)
                .SetEase(Ease.Linear);
    }
}