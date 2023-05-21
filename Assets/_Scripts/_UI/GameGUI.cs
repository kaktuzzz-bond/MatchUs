using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class GameGUI : Singleton<GameGUI>
{
    public event Action<bool> OnButtonPressPermission;

    public GameButton AddButton { get; private set; }

    public GameButton SpecialButton { get; private set; }

    public GameButton HintButton { get; private set; }

    public GameButton UndoButton { get; private set; }

#region GAME OBJECTS

    [SerializeField]
    private RectTransform header;

    [SerializeField]
    private RectTransform footer;

    [SerializeField]
    private InfoPanel infoPanel;

    [SerializeField]
    private FaderUI fader;

    [SerializeField]
    private PausePopup pausePopup;

    [SerializeField]
    private GameOverPopup gameOverPopup;

    [SerializeField] [FoldoutGroup("Game Buttons")]
    private Button add;

    [SerializeField] [FoldoutGroup("Game Buttons")]
    private Button special;

    [SerializeField] [FoldoutGroup("Game Buttons")]
    private Button hint;

    [SerializeField] [FoldoutGroup("Game Buttons")]
    private Button undo;

    [SerializeField] [FoldoutGroup("Text")]
    private TextMeshProUGUI timer;

    [SerializeField] [FoldoutGroup("Text")]
    private TextMeshProUGUI score;

    [SerializeField] [FoldoutGroup("Header Buttons")]
    private Button pause;

    [SerializeField] [FoldoutGroup("Header Buttons")]
    private Button shop;

    public FaderUI Fader => fader;

    public string GameTime => timer.text;

    public string GameScore => score.text;

    private GameManager _gameManager;

#endregion


    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }


    public void SetButtonPressPermission(bool isAllowed)
    {
        OnButtonPressPermission?.Invoke(isAllowed);
    }


    public Vector3[] GetHeaderCorners() => Utils.GetRectWorldCorners(header);


    public Vector3[] GetFooterCorners() => Utils.GetRectWorldCorners(footer);


    public bool IsGameAreaPosition(Vector3 position)
    {
        bool checkX = position.x > -0.5f &&
                      position.x < _gameManager.gameData.width + 0.5f;

        bool checkY = position.y < GetHeaderCorners()[0].y &&
                      position.y > GetFooterCorners()[1].y;

        return checkX && checkY;
    }


    private void Init()
    {
        // game buttons
        add.onClick.AddListener(()=> ChipController.Instance.AddChips().Forget());

        AddButton = add.GetComponent<GameButton>();

        special.onClick.AddListener(ChipController.Instance.ShuffleChips);

        SpecialButton = special.GetComponent<GameButton>();

        hint.onClick.AddListener(ChipController.Instance.ShowHints);

        HintButton = hint.GetComponent<GameButton>();

        undo.onClick.AddListener(ChipController.Instance.UndoCommand);

        UndoButton = undo.GetComponent<GameButton>();

        // header buttons
        pause.onClick.AddListener(() => PauseClickedAsync().Forget());

        shop.onClick.AddListener(ShopClicked);

        infoPanel.Init();
    }


#region ACTIONS

    private void GameOver()
    {
        gameOverPopup.ShowPopupAsync().Forget();
    }


    private async UniTaskVoid PauseClickedAsync()
    {
        Debug.Log("PAUSE");

        //SetButtonPressPermission(false);

        GameManager.Instance.PauseGame();

        pausePopup.ShowPopupAsync().Forget();

        await Fader.FadeInEffect();
    }


    private void ShopClicked()
    {
        Debug.Log("SHOP");
    }


    public void ShowInfo() => infoPanel.Show().Forget();


    public void HideInfo() => infoPanel.Hide().Forget();


    public async UniTask SetupGUIAndFadeOut()
    {
        Init();

        await fader.FadeOutEffect();
    }

#endregion

#region TEXT

    public void UpdateScore(int scoreValue)
    {
        score.text = scoreValue.ToString();
    }


    public void UpdateTime(float timeCounter)
    {
        timer.text = Timer.FormatTime(timeCounter);
    }

#endregion

#region ENABLE / DISABLE

    private void OnEnable()
    {
        _gameManager.OnGameOver += GameOver;
    }


    private void OnDisable()
    {
        _gameManager.OnGameOver -= GameOver;
    }

#endregion
}