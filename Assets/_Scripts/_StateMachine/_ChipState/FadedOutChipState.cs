using UnityEngine;
using DG.Tweening;
public class FadedOutChipState : IChipState
{
    public void Enter(Chip chip)
    {
        Debug.LogWarning("Fade Out Enter");
        
        chip.Renderer
                .DOFade(0.15f, Chip.FadeTime)
                .SetEase(Ease.Linear);
        
    }
}