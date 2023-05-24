using NonMono;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainSceneGUI : Singleton<MainSceneGUI>
    {
        [SerializeField] [TabGroup("Main Menu")]
        private Button easyMode;

        [SerializeField] [TabGroup("Main Menu")]
        private Button normalMode;

        [SerializeField] [TabGroup("Main Menu")]
        private Button hardMode;

        [SerializeField] [TabGroup("Main Menu")]
        private Button resumeTheGame;

        [SerializeField] [TabGroup("Main Menu")]
        private Button testMode;

        private GameManager _gameManager;


        private void Awake()
        {
            _gameManager = GameManager.Instance;

            easyMode.onClick.AddListener(
                    () =>
                    {
                        _gameManager.gameData.difficultyLevel = GameData.DifficultyLevel.Easy;

                        _gameManager.gameData.SetStartArrayInfos(ChipInfo.GetStartChipInfoArray());

                        _gameManager.GameFiniteStateMachine.Loading();
                    });

            normalMode.onClick.AddListener(
                    () =>
                    {
                        _gameManager.gameData.difficultyLevel = GameData.DifficultyLevel.Normal;

                        _gameManager.gameData.SetStartArrayInfos(ChipInfo.GetStartChipInfoArray());

                        _gameManager.GameFiniteStateMachine.Loading();
                    });

            hardMode.onClick.AddListener(
                    () =>
                    {
                        _gameManager.gameData.difficultyLevel = GameData.DifficultyLevel.Hard;

                        _gameManager.gameData.SetStartArrayInfos(ChipInfo.GetStartChipInfoArray());

                        _gameManager.GameFiniteStateMachine.Loading();
                    });

            resumeTheGame.onClick.AddListener(
                    () => { Debug.LogWarning("Load saved data process"); });

            testMode.onClick.AddListener(
                    () =>
                    {
                        _gameManager.gameData.difficultyLevel = GameData.DifficultyLevel.Test;

                        _gameManager.gameData.SetStartArrayInfos(ChipInfo.GetStartChipInfoArray());

                        _gameManager.GameFiniteStateMachine.Loading();
                    });
        }
    }
}