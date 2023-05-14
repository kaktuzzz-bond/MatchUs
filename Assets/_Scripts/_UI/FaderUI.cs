using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FaderUI : MonoBehaviour
{
    [MinValue(0)]
    public float fadeTime = 1f;

    [PropertyRange(0, 1)]
    public float maxFadeValue = 0.8f;

    [SerializeField]
    private Image fade;


    private void Awake()
    {
        fade.gameObject.SetActive(true);
    }


    public async UniTask FadeOutEffect()
    {
        await fade
                .DOFade(0, fadeTime)
                .SetEase(Ease.InCubic)
                .ToUniTask();

        fade.gameObject.SetActive(false);
    }


    public async UniTask FadeInEffect()
    {
        fade.gameObject.SetActive(true);

        await fade
                .DOFade(maxFadeValue, fadeTime)
                .SetEase(Ease.InCubic)
                .ToUniTask();
    }
}