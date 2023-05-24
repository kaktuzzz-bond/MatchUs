using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class InfoPanel : MonoBehaviour
    {
        public bool IsShown { get; private set; }

        private const float InfoJump = 2f;

        private const float InfoJumpDuration = 0.6f;

        private float _hiddenPositionY;

        private float _showedPositionY;


        public void Init()
        {
            _hiddenPositionY = transform.position.y;

            _showedPositionY = _hiddenPositionY + InfoJump;
        }


        public async UniTask Show()
        {
            if (IsShown) return;

            GameGUI.Instance.HintButton.SetInteractivity(false);

            Debug.Log("Show Info");

            gameObject.SetActive(true);

            IsShown = true;

            await transform
                    .DOMoveY(_showedPositionY, InfoJumpDuration)
                    .SetEase(Ease.OutBack)
                    .ToUniTask();
        }


        public async UniTask Hide()
        {
            if (!IsShown) return;

            Debug.Log("Hide Info");

            await transform
                    .DOMoveY(_hiddenPositionY, InfoJumpDuration)
                    .SetEase(Ease.InBack)
                    .ToUniTask();

            gameObject.SetActive(false);

            IsShown = false;

            GameGUI.Instance.HintButton.SetInteractivity(true);
        }
    }
}