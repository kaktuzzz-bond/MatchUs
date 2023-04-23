using UnityEngine;
using DG.Tweening;
public class ChipFadedInState : IChip
{
    public override void Enter(ChipStateManager chip)
    {
        chip.Renderer
                .DOFade(1f, chip.fadeTime)
                .SetEase(Ease.Linear);
        
    }
}