using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

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

        easyMode.onClick.AddListener(() =>
        {
            _gameManager.gameData.difficultyLevel = DifficultyLevel.Easy;

            GameManager.Instance.GameFiniteStateMachine.Loading();

        });

        normalMode.onClick.AddListener(() =>
        {
            _gameManager.gameData.difficultyLevel = DifficultyLevel.Normal;

            GameManager.Instance.GameFiniteStateMachine.Loading();
        });

        hardMode.onClick.AddListener(() =>
        {
            _gameManager.gameData.difficultyLevel = DifficultyLevel.Hard;

            GameManager.Instance.GameFiniteStateMachine.Loading();
        });
        
        resumeTheGame.onClick.AddListener(() =>
        {
            Debug.Log("Continue");
        });

        testMode.onClick.AddListener(() =>
        {
            _gameManager.gameData.difficultyLevel = DifficultyLevel.Test;

            GameManager.Instance.GameFiniteStateMachine.Loading();
        });
    }
}