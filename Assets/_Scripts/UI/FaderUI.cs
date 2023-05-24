using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FaderUI : MonoBehaviour
    {
        [MinValue(0)]
        public float fadeTime;

        [PropertyRange(0, 1)]
        public float maxFadeValue;

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
            Debug.Log("Fade In Effect");
        
            fade.gameObject.SetActive(true);

            await fade
                    .DOFade(maxFadeValue, fadeTime)
                    .SetEase(Ease.InCubic)
                    .ToUniTask();
        }
    }
}