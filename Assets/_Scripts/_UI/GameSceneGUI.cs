using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneGUI : Singleton<GameSceneGUI>
{
    public event Action OnFaderDisappeared;

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
        add.onClick.AddListener(ChipController.AddChips);

        special.onClick.AddListener(ChipController.ShuffleChips);

        hint.onClick.AddListener(PointerController.Instance.ShowHints);

        undo.onClick.AddListener(() => StartCoroutine(ChipController.Instance.Log.UndoCommand()));
    }

#endregion

#region FADE IN / FADE OUT SCREEN

    private void FadeInEffect()
    {
        fader
                .DOFade(0, 0.2f)
                .SetEase(Ease.InCubic)
                .onComplete += () =>
        {
            fader.gameObject.SetActive(false);

            OnFaderDisappeared?.Invoke();
        };
    }

#endregion

#region ENABLE / DISABLE

    private void OnEnable()
    {
        CameraController.Instance.OnCameraSetup += FadeInEffect;
    }


    private void OnDisable()
    {
        CameraController.Instance.OnCameraSetup -= FadeInEffect;
    }

#endregion
}