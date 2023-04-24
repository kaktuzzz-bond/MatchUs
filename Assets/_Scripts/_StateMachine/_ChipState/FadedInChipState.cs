using UnityEngine;
using DG.Tweening;
public class FadedInChipState : IChipState
{
    public void Enter(Chip chip)
    {
        //Debug.Log("Fade In Enter");
        
        chip.Renderer
                .DOFade(1f, Chip.FadeTime)
                .SetEase(Ease.Linear);
        
    }
}