using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneGUI : Singleton<GameSceneGUI>
{
    public event Action OnFadedIn;

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

    private CameraController _cameraController;

    private ChipController _chipController;
    
    private Board _board;


    private void Awake()
    {
        _cameraController = CameraController.Instance;
        _chipController = ChipController.Instance;
        _board = Board.Instance;

        add.onClick.AddListener(() => _chipController.AddChips());
        
        undo.onClick.AddListener(() => _chipController.Log.UndoCommand());
        
        //special.onClick.AddListener(() => _commandLogger.Execute(new SpecialActionCommand()));
    }


    public Vector3[] GetHeaderCorners()
    {
        return Utils.GetRectWorldCorners(header);
    }


    public Vector3[] GetFooterCorners()
    {
        return Utils.GetRectWorldCorners(footer);
    }


    public bool IsGameAreaPosition(Vector3 position)
    {
        bool checkX = position.x > -0.5f &&
                      position.x < _board.Width + 0.5f;

        bool checkY = position.y < GetHeaderCorners()[0].y &&
                      position.y > GetFooterCorners()[1].y;

        return checkX && checkY;
    }


    private void FadeIn()
    {
        fader
                .DOFade(0, 0.5f)
                .SetEase(Ease.InSine);

        OnFadedIn?.Invoke();
    }


#region Enable / Disable

    private void OnEnable()
    {
        _cameraController.OnCameraSetup += FadeIn;
    }


    private void OnDisable()
    {
        _cameraController.OnCameraSetup -= FadeIn;
    }

#endregion
}