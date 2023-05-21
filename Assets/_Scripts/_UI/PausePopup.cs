using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : Popup
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

        Init();
    }


    private async UniTask ResumeGameAsync()
    {
        await HidePopupAsync();

        await _gameGUI.Fader.FadeOutEffect();

        _gameGUI.SetButtonPressPermission(true);

        GameManager.Instance.ResumeGame();
    }


    private async UniTask GoHomeAsync()
    {
        await HidePopupAsync();

        GameManager.Instance.ExitGame();
    }


    private async UniTaskVoid Restart()
    {
        await ChipController.Instance.ChipRegistry.ResetRegistry();

        await HidePopupAsync();
        
        await _gameGUI.Fader.FadeOutEffect();
        
        //ChipController.Instance.DrawStartArray();
    }


    private void ShowInfo()
    {
        Debug.Log("SHOW INFO");
    }
}