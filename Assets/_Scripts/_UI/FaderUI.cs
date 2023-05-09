using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FaderUI : MonoBehaviour
{
    [SerializeField]
    private Image fade;


    private void Awake()
    {
        fade.DOFade(1f, 0);
    }


    public void FadeOutEffect(Action action)
    {
        fade
                .DOFade(0, 0.2f)
                .SetEase(Ease.InCubic)
                .onComplete += () =>
        {
            action?.Invoke();

            fade.gameObject.SetActive(false);
        };
    }


    public void FadeInEffect(Action action)
    {
        fade.gameObject.SetActive(true);

        fade
                .DOFade(0.8f, 0.2f)
                .SetEase(Ease.InCubic)
                .onComplete += () => { action?.Invoke(); };
    }
}