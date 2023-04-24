using UnityEngine;
using DG.Tweening;
public class ChipStateFadedInState : IChipState
{
    public void Enter(Chip chip)
    {
        chip.Renderer
                .DOFade(1f, Chip.FadeTime)
                .SetEase(Ease.Linear);
        
    }
}