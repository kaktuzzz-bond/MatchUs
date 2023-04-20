using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraController : Singleton<CameraController>
{
    [HorizontalGroup("Split", Title = "On Hold Properties")]
    [SerializeField, BoxGroup("Split/Duration Multiplier"), HideLabel]
    private float durationMultiplier = 14f;

    [SerializeField, BoxGroup("Split/On Hold Speed"), HideLabel]
    private float onHoldCamSpeed = 2f;

    [SerializeField, BoxGroup("Split/On Hold Delay"), HideLabel]
    private float onHoldCamDelay = 0.1f;

    private Camera _camera;

    private Board _board;

    private InputManager _inputManager;

    private GameUIController _gameUIController;

    private Vector3 _startCameraPosition;

    // Limits of camera movement
    private Vector3 _topBoundPoint;

    private Vector3 _bottomBoundPoint;


    private void Awake()
    {
        _board = Board.Instance;

        _inputManager = InputManager.Instance;

        _gameUIController = GameUIController.Instance;

        _camera = Camera.main;
    }


    private void Setup()
    {
        SetOrthographicSize();

        StartCoroutine(Test());
    }


    private IEnumerator Test()
    {
        yield return new WaitForEndOfFrame();

        SetBounds();

        SetInitialPosition();
    }


    private void StartInput(Vector3 position)
    {
        Debug.Log("StartInput");

        _camera.transform.DOKill();

        _startCameraPosition = _camera.transform.position;
    }


    private void DoOnTap(Vector3 position)
    {
        // Debug.Log($"{nameof(DetectTile)} in {position}");
    }


    private void MoveOnSwipe(float tweenEndValue, float tweenDuration)
    {
        float targetValue = _camera.transform.position.y - tweenEndValue;

        float targetDuration = tweenDuration * durationMultiplier;

        _camera.transform
                .DOMoveY(targetValue, targetDuration)
                .SetEase(Ease.OutQuad);
    }


    private void MoveOnHold(Vector2 offset)
    {
        float targetValue = _startCameraPosition.y - offset.y * onHoldCamSpeed;

        _camera.transform
                .DOMoveY(targetValue, onHoldCamDelay)
                .SetEase(Ease.Linear);
    }


    private void SetOrthographicSize()
    {
        float aspectRatio = (float)Screen.height / Screen.width;

        _camera.orthographicSize = (_board.Width + 0.5f) * aspectRatio * 0.5f;

        Debug.Log(" Complete Orthographic " + _camera.orthographicSize);
    }


    private void SetBounds()
    {
        var rectHeader = _gameUIController.GetHeaderCorners();

        float headerHeight = rectHeader[1].y - rectHeader[0].y;

        _topBoundPoint = new Vector3(
                x: (_board.Width - 1f) * 0.5f,
                y: headerHeight - _camera.orthographicSize + 0.5f,
                z: _camera.transform.position.z);

        _bottomBoundPoint = _topBoundPoint;
    }


    private void SetInitialPosition()
    {
        _camera.transform.position = _topBoundPoint;
    }


    #region Enable / Disable

    private void OnEnable()
    {
        _board.OnTilesGenerated += Setup;
        _inputManager.OnTapDetected += DoOnTap;
        _inputManager.OnSwipeDetected += MoveOnSwipe;
        _inputManager.OnTouchStarted += StartInput;
        _inputManager.OnHoldPressed += MoveOnHold;
    }


    private void OnDisable()
    {
        _board.OnTilesGenerated -= Setup;
        _inputManager.OnTapDetected -= DoOnTap;
        _inputManager.OnSwipeDetected -= MoveOnSwipe;
        _inputManager.OnTouchStarted -= StartInput;
        _inputManager.OnHoldPressed -= MoveOnHold;
    }

    #endregion
}