using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverPopup : Popup
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

        restart.onClick.AddListener(Restart);

        _gameGUI = GameGUI.Instance;

        Init();
    }


    public override async UniTask ShowPopupAsync()
    {
        await base.ShowPopupAsync();

        time.text = GameGUI.Instance.GameTime;

        score.text = GameGUI.Instance.GameScore;
    }


    private void Restart()
    {
        Debug.Log("RESTART");
    }


    private async UniTask GoHomeAsync()
    {
        await HidePopupAsync();

        GameManager.Instance.ExitGame();
    }
}