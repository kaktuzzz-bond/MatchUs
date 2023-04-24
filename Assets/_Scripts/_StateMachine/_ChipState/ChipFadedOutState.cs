using UnityEngine;
using DG.Tweening;
public class ChipFadedOutState : IChip
{
    public void Enter(Chip chip)
    {
        chip.Renderer
                .DOFade(0.15f, Chip.FadeTime)
                .SetEase(Ease.Linear);
        
    }
}