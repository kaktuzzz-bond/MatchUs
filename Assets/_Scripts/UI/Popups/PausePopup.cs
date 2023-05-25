using Cysharp.Threading.Tasks;
using Game;
using NonMono;
using NonMono.Commands;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class PausePopup : PopupBase
    {
        [SerializeField]
        private Button close;

        [SerializeField]
        private Button info;

        [SerializeField]
        private Button home;

        [SerializeField]
        private Button restart;

        private GameGUI _gameGUI;


        private void Awake()
        {
            close.onClick.AddListener(() => ResumeGameAsync().Forget());

            info.onClick.AddListener(ShowInfo);

            home.onClick.AddListener(() => GoHomeAsync().Forget());

            restart.onClick.AddListener(() => Restart().Forget());

            _gameGUI = GameGUI.Instance;
        }


        private async UniTask ResumeGameAsync()
        {
            await HidePopupAsync();

            await _gameGUI.Fader.FadeOutEffect();

            _gameGUI.SetButtonPressPermission(true);

            GameManager.Instance.ResumeGame();
        }


        protected override async UniTask Restart()
        {
            ChipController.Instance.Restart();

            await base.Restart();
        }


        private void ShowInfo()
        {
            Debug.Log("SHOW INFO");
        }
    }
}