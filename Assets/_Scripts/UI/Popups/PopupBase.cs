using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI.Popups
{
    public abstract class PopupBase : MonoBehaviour
    {
        private const float PopDuration = 0.5f;


        protected void Init()
        {
            // gameObject.SetActive(false);
            //
            // transform.localScale = Vector3.zero;
        }


        public virtual async UniTask ShowPopupAsync()
        {
      
            await GameGUI.Instance.Fader.FadeInEffect();

            gameObject.SetActive(true);

            await transform.DOScale(Vector3.one, PopDuration)
                    .SetEase(Ease.OutBack)
                    .ToUniTask();
        }


        protected async UniTask HidePopupAsync()
        {
            await transform.DOScale(Vector3.zero, PopDuration)
                    .SetEase(Ease.InBack)
                    .ToUniTask();

            // await GameGUI.Instance.Fader.FadeOutEffect();

            gameObject.SetActive(false);
        }
    }
}