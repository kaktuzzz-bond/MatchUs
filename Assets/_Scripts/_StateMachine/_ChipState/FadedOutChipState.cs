using UnityEngine;
using DG.Tweening;
public class FadedOutChipState : IChipState
{
    public void Enter(Chip chip)
    {
        //Debug.Log("Fade Out Enter");
        
        chip.Renderer
                .DOFade(0.15f, Chip.FadeTime)
                .SetEase(Ease.Linear);
        
    }
}