using System;
using Cysharp.Threading.Tasks;
using NonMono;
using NonMono.Game_FSM;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public event Action OnGameOver;

        public GameData gameData;

        //[ShowInInspector]
        //public IGameState CurrentGameState => GameFiniteStateMachine.CurrentGameState;

        public GameFiniteStateMachine GameFiniteStateMachine { get; private set; }

        private int _score;

        private float _timerCounter;

        private bool _isTimerOn;

        private int Score
        {
            get => _score;
            set
            {
                _score = value;
                GameGUI.Instance.UpdateScore(_score);
            }
        }


        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            GameFiniteStateMachine = new GameFiniteStateMachine();
        }


        public void AddScore(int score)
        {
            Score += score;
        }


        public void StartGame()
        {
            Debug.Log("GAME STARTED!");

            ResetGameStats();

            EnableTimer();

            InputManager.Instance.SetPlayerInput(true);
        }


        public void PauseGame()
        {
            Debug.Log("GAME PAUSED");

            DisableTimer();
        }


        public void ResumeGame()
        {
            Debug.Log("GAME RESUMED");

            EnableTimer();
        }


        public void EndGame()
        {
            Debug.Log("GAME OVER!");

            OnGameOver?.Invoke();

            InputManager.Instance.SetPlayerInput(false);

            DisableTimer();
        }


        public void ExitGame()
        {
            GameFiniteStateMachine.Loading();
        }


        private void EnableTimer()
        {
            CountTimeAsync().Forget();
        }


        public void DisableTimer()
        {
            _isTimerOn = false;
        }


        private void ResetGameStats()
        {
            Score = 0;

            _timerCounter = 0;
        }


        private async UniTaskVoid CountTimeAsync()
        {
            if (GameFiniteStateMachine.CurrentGameState.GetType() != typeof(ActiveGameState)) return;

            Debug.Log("Timer starts");

            _isTimerOn = true;

            while (_isTimerOn)
            {
                _timerCounter++;

                try
                {
                    GameGUI.Instance.UpdateTime(_timerCounter);
                }
                catch (NullReferenceException e)
                {
                    Debug.LogWarning(e);

                    DisableTimer();
                }

                await UniTask.Delay(1000);
            }

            Debug.Log("Timer stopped");
        }
    }
}