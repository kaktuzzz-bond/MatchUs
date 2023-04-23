using UnityEngine;
using DG.Tweening;

public class ChipDisabledState : IChip
{
    public override void Enter(ChipStateManager chip)
    {
        Vector3 initPos = chip.transform.position;

        Vector3 targetPos = new(initPos.x, initPos.y + 0.5f, initPos.z);

        chip.Renderer
                .DOFade(0f, ChipStateManager.FadeTime)
                .SetEase(Ease.Linear);

        chip.transform
                .DOMoveY(targetPos.y, ChipStateManager.FadeTime)
                .SetEase(Ease.Linear)
                .onComplete += () => chip.gameObject.SetActive(false);
    }
}