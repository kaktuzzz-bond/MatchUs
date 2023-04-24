using DG.Tweening;
using UnityEngine;

public class InitializedChipState : IChipState
{
    public void Enter(Chip chip)
    {
        //Debug.Log("Initialize state Enter");
        
        Vector3 targetPos = Board.Instance[chip.BoardPosition.x, chip.BoardPosition.y].position;

        Vector3 initPos = new(targetPos.x, targetPos.y + 0.5f, targetPos.z);

        chip.transform.position = initPos;

        chip.Renderer
                .DOFade(0, 0);

        chip.gameObject.SetActive(true);

        chip.transform
                .DOMoveY(targetPos.y, Chip.FadeTime)
                .SetEase(Ease.Linear);
        
        chip.Renderer
                .DOFade(1f, Chip.FadeTime)
                .SetEase(Ease.Linear);
    }
}