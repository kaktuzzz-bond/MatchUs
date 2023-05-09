#define ENABLE_LOGS
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameGUI : Singleton<GameGUI>
{
    public event Action OnFadeOutEffected;

    public event Action OnFadeInEffected;

#region GAME OBJECT LINKS

    [SerializeField]
    private RectTransform header;

    [SerializeField]
    private RectTransform footer;

    [SerializeField]
    private Image fader;

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

#endregion


    public Vector3[] GetHeaderCorners() => Utils.GetRectWorldCorners(header);


    public Vector3[] GetFooterCorners() => Utils.GetRectWorldCorners(footer);


    public bool IsGameAreaPosition(Vector3 position)
    {
        bool checkX = position.x > -0.5f &&
                      position.x < Board.Instance.Width + 0.5f;

        bool checkY = position.y < GetHeaderCorners()[0].y &&
                      position.y > GetFooterCorners()[1].y;

        return checkX && checkY;
    }


#region INITIALIZATION

    public void Init()
    {
        // game buttons
        add.onClick.AddListener(ChipController.Instance.AddChips);

        special.onClick.AddListener(ChipController.Instance.ShuffleChips);

        hint.onClick.AddListener(PointerController.Instance.ShowHints);

        undo.onClick.AddListener(() => StartCoroutine(ChipController.Instance.Log.UndoCommand()));

        // header buttons
        pause.onClick.AddListener(PauseClicked);

        shop.onClick.AddListener(ShopClicked);
    }

#endregion

#region BUTTON ACTIONS

    private void PauseClicked()
    {
        Logger.DebugWarning("PAUSE");

        GameManager.Instance.PauseGame();
    }


    private void ShopClicked()
    {
        Logger.DebugWarning("SHOP");

        GameManager.Instance.PauseGame();
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

#region FADE IN / FADE OUT SCREEN

    private void FadeOutEffect()
    {
        fader
                .DOFade(0, 0.2f)
                .SetEase(Ease.InCubic)
                .onComplete += () =>
        {
            fader.gameObject.SetActive(false);

            OnFadeOutEffected?.Invoke();
        };
    }


    private void FadeInEffect()
    {
        fader.gameObject.SetActive(true);

        fader
                .DOFade(0.8f, 0.2f)
                .SetEase(Ease.InCubic)
                .onComplete += () => { OnFadeInEffected?.Invoke(); };
    }

#endregion

#region ENABLE / DISABLE

    private void OnEnable()
    {
        CameraController.Instance.OnCameraSetup += FadeOutEffect;
    }


    private void OnDisable()
    {
        CameraController.Instance.OnCameraSetup -= FadeOutEffect;
    }

#endregion
}