using System;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraController : Singleton<CameraController>
{
    [SerializeField]
    private float durationMultiplier = 14f;

    [SerializeField]
    private float durationOnHold = 0.1f;

    private Camera _camera;

    private Board _board;

    private InputManager _inputManager;

    private Vector3 _cameraOffset;


    private void Awake()
    {
        _board = Board.Instance;

        _inputManager = InputManager.Instance;

        _camera = Camera.main;
    }


    private void Setup()
    {
        SetOrthographicSize();
        SetInitialPosition();
    }


    private void StartInput(Vector3 position)
    {
        Debug.Log("StartInput");

        _camera.transform.DOKill();

        Debug.Log("CamPos: " + _camera.transform.position);
        Debug.Log("FingerPos: " + position);

        _cameraOffset = position - _camera.transform.position;
        Debug.Log("Offset: " + _cameraOffset);
    }


    private void DoOnTap(Vector3 position)
    {
        // Debug.Log($"{nameof(DetectTile)} in {position}");
    }


    private void MoveOnSwipe(float tweenEndValue, float tweenDuration)
    {
        Debug.Log("MoveOnSwipe ");

        float targetValue = _camera.transform.position.y - tweenEndValue;

        float targetDuration = tweenDuration * durationMultiplier;

        _camera.transform
                .DOMoveY(targetValue, targetDuration)
                .SetEase(Ease.OutQuad);
    }


    private void MoveOnHold(Vector3 position)
    {
        Debug.LogWarning($"MoveOnHold " + position);

        _camera.transform
                .DOMoveY(position.y - _cameraOffset.y, durationOnHold)
                .SetEase(Ease.Linear);
    }


    private void SetOrthographicSize()
    {
        // Bounds bounds = _board[0, 0].GetComponentInChildren<SpriteRenderer>().bounds;
        //
        // _camera.orthographicSize = (bounds.size.x * _board.Width + 1f) * Screen.height / Screen.width * 0.5f;

        _camera.orthographicSize = (_board.Width + 1f) * Screen.height / Screen.width * 0.5f;
    }


    private void SetInitialPosition() =>
            _camera.transform.position = new Vector3(
                    (_board.Width - 1f) * 0.5f,
                    0f,
                    _camera.nearClipPlane);


    #region Enable / Disable

    private void OnEnable()
    {
        _board.OnTilesGenerated += Setup;
        _inputManager.OnTapDetected += DoOnTap;
        _inputManager.OnSwipeDetected += MoveOnSwipe;
        _inputManager.OnTouchStarted += StartInput;
    }


    private void OnDisable()
    {
        _board.OnTilesGenerated -= Setup;
        _inputManager.OnTapDetected -= DoOnTap;
        _inputManager.OnSwipeDetected -= MoveOnSwipe;
        _inputManager.OnTouchStarted -= StartInput;
    }

    #endregion
}