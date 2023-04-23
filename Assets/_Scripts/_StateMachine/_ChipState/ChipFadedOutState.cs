using UnityEngine;
using DG.Tweening;
public class ChipFadedOutState : IChip
{
    public override void Enter(ChipStateManager chip)
    {
        chip.Renderer
                .DOFade(0.15f, ChipStateManager.FadeTime)
                .SetEase(Ease.Linear);
        
    }
}