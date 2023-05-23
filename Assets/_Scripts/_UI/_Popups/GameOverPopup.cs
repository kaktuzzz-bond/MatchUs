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
        await ChipRegistry.ResetRegistry();

        await HidePopupAsync();
        
        await _gameGUI.Fader.FadeOutEffect();
        
        //ChipController.Instance.DrawStartArray();
    }


    private async UniTask GoHomeAsync()
    {
        await HidePopupAsync();

        GameManager.Instance.ExitGame();
    }
}