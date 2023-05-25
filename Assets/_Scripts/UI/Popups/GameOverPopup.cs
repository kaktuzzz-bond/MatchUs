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
        }


        public override async UniTask ShowPopupAsync()
        {
            time.text = GameGUI.Instance.GameTime;

            score.text = GameGUI.Instance.GameScore;

            await base.ShowPopupAsync();
        }


        protected override async UniTask Restart()
        {
            ChipController.Instance.Restart();

            await base.Restart();
        }
        
    }
}