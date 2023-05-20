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
            _gameManager.SetDifficultyAndLoad(DifficultyLevel.Easy);
            
        });

        normalMode.onClick.AddListener(() =>
        {
            _gameManager.SetDifficultyAndLoad(DifficultyLevel.Normal);
        });

        hardMode.onClick.AddListener(() =>
        {
            _gameManager.SetDifficultyAndLoad(DifficultyLevel.Hard);
        });
        
        resumeTheGame.onClick.AddListener(() =>
        {
            Debug.Log("Continue");
        });

        testMode.onClick.AddListener(() =>
        {
            _gameManager.SetDifficultyAndLoad(DifficultyLevel.Test);
        });
    }
}