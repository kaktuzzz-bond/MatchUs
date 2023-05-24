using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game;
using NonMono;
using NonMono.Commands;
using UnityEngine;

namespace UI.Popups
{
    public abstract class PopupBase : MonoBehaviour
    {
        private const float PopDuration = 0.5f;

        


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
        
        
        protected virtual async UniTask Restart()
        {
          

            await HidePopupAsync();

            await GameGUI.Instance.Fader.FadeOutEffect();

            await CommandLogger
                    .AddCommand(new AddChipsCommand(GameManager.Instance.gameData.StartArrayInfos));
        }


        protected virtual async UniTask GoHomeAsync()
        {
            ChipController.Instance.GoHome();
            
            await HidePopupAsync();

            GameManager.Instance.ExitGame();
        }
    }
}