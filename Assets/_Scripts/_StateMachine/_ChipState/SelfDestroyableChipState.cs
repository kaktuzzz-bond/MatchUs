using DG.Tweening;
using UnityEngine;

public class SelfDestroyableChipState : IChipState
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
                .onComplete += () =>
        {
            ChipRegistry.Instance.Unregister(chip);
                
            chip.SelfDestroy();
        };

           
    }
}