using Cysharp.Threading.Tasks;
using Game;
using NonMono;
using NonMono.Commands;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class GameOverPopup : PopupBase
    {
        [SerializeField]
        private Button restart;

        [SerializeField]
        private Button home;

        [SerializeField]
        private TextMeshProUGUI time;

        [SerializeField]
        private TextMeshProUGUI score;

        private GameGUI _gameGUI;


        private void Awake()
        {
            home.onClick.AddListener(() => GoHomeAsync().Forget());

            restart.onClick.AddListener(() => Restart().Forget());

            _gameGUI = GameGUI.Instance;

            Init();
        }


        public override async UniTask ShowPopupAsync()
        {
            time.text = GameGUI.Instance.GameTime;

            score.text = GameGUI.Instance.GameScore;

            await base.ShowPopupAsync();
        }


        private async UniTaskVoid Restart()
        {
            // ChipController.Instance.Restart();
            //
            // await HidePopupAsync();
            //
            // await _gameGUI.Fader.FadeOutEffect();
            
            ChipController.Instance.Restart();
            
            await HidePopupAsync();

            await _gameGUI.Fader.FadeOutEffect();
            
            await CommandLogger
                    .AddCommand(new AddChipsCommand(GameManager.Instance.gameData.StartArrayInfos));
        }


        private async UniTask GoHomeAsync()
        {
            CommandLogger.Reset();

            ChipRegistry.Reset();
            
            await HidePopupAsync();

            GameManager.Instance.ExitGame();
        }
    }
}