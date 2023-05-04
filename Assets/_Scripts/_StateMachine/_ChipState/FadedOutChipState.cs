using UnityEngine;
using DG.Tweening;

public class FadedOutChipState : IChipState
{
    public void Enter(Chip chip)
    {
        //Debug.Log("Fade Out Enter");

        chip.Renderer
                .DOFade(0.12f, Chip.FadeTime * 2)
                .SetEase(Ease.Linear);
    }
}