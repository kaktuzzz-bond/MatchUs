using UnityEngine;
using DG.Tweening;

public class ChipStateDisabledState : IChipState
{
    public void Enter(Chip chip)
    {
        Vector3 initPos = chip.transform.position;

        Vector3 targetPos = new(initPos.x, initPos.y + 0.5f, initPos.z);

        chip.Renderer
                .DOFade(0f, Chip.FadeTime)
                .SetEase(Ease.Linear);

        chip.transform
                .DOMoveY(targetPos.y, Chip.FadeTime)
                .SetEase(Ease.Linear)
                .onComplete += () => chip.gameObject.SetActive(false);
    }
}