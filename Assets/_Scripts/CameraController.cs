using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraController : Singleton<CameraController>
{
    [HorizontalGroup("Split", Title = "On Hold Properties")]
    [SerializeField, PropertyRange(0f, 2f), BoxGroup("Split/Velocity Threshold"), HideLabel]
    private float cameraVelocityThreshold = 0.1f;

    [SerializeField, MinValue(0), BoxGroup("Split/On Hold Speed"), HideLabel]
    private float endTouchMoveDuration = 1f;

    [SerializeField, MinValue(0), BoxGroup("Split/On Hold Delay"), HideLabel]
    private float onHoldMoveDuration = 0.1f;

#region Component Links

    private Camera _camera;

    private Board _board;

    private InputManager _inputManager;

    private GameUIController _gameUIController;

#endregion

    private Vector3 _startCameraPosition;

    private Vector3 _startTouchFingerPosition;

    private bool _isStopMovement;

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


    private void Update()
    {
        LimitCameraMovementToBounds();
    }


    private void DoOnStartTouch(Vector3 position)
    {
        _isStopMovement = Mathf.Abs(_camera.velocity.y) > cameraVelocityThreshold;

        _camera.transform.DOKill();

        _startCameraPosition = _camera.transform.position;

        _startTouchFingerPosition = position;
    }


    private void DoOnTapDetected(Vector3 position)
    {
        if (_isStopMovement) return;

        Vector2Int boardCoords = Utils.ConvertWorldToBoardCoordinates(position);
        
        Debug.LogWarning($"TAP in world point {boardCoords}");
    }


    private void MovementOnHoldTouch(Vector3 position)
    {
        float offset = position.y - _startTouchFingerPosition.y;

        float targetValue = _startCameraPosition.y - offset;

        _camera.transform
                .DOMoveY(targetValue, onHoldMoveDuration)
                .SetEase(Ease.Linear);
    }


    private void MovementOnEndTouch()
    {
        float currentValue = _camera.transform.position.y;

        float verticalVelocity = _camera.velocity.y;

        float targetValue = currentValue + verticalVelocity;

        _camera.transform
                .DOMoveY(targetValue, endTouchMoveDuration)
                .SetEase(Ease.OutQuad);
    }


    private void LimitCameraMovementToBounds()
    {
        if (_camera.transform.position.y > _topBoundPoint.y)
        {
            _camera.transform
                    .DOMoveY(_topBoundPoint.y, onHoldMoveDuration)
                    .SetEase(Ease.Linear);
        }

        if (_camera.transform.position.y < _bottomBoundPoint.y)
        {
            _camera.transform
                    .DOMoveY(_bottomBoundPoint.y, onHoldMoveDuration)
                    .SetEase(Ease.Linear);
        }
    }


    private void SetupCamera()
    {
        StartCoroutine(SetupCameraRoutine());
    }


    private IEnumerator SetupCameraRoutine()
    {
        SetOrthographicSize();

        yield return new WaitForEndOfFrame();

        SetBounds();

        SetInitialPosition();
    }


    private void SetOrthographicSize()
    {
        float aspectRatio = (float)Screen.height / Screen.width;

        _camera.orthographicSize = (_board.Width + 0.5f) * aspectRatio * 0.5f;
    }


    private void SetBounds()
    {
        var rectHeader = _gameUIController.GetHeaderCorners();

        float headerHeight = rectHeader[1].y - rectHeader[0].y;

        _topBoundPoint = new Vector3(
                x: (_board.Width - 1f) * 0.5f,
                y: headerHeight - _camera.orthographicSize + 0.5f,
                z: _camera.transform.position.z);

        _bottomBoundPoint = _topBoundPoint - new Vector3(0, 25f, 0);
    }


    private void SetInitialPosition()
    {
        _camera.transform.position = _topBoundPoint;
    }


#region Enable / Disable

    private void OnEnable()
    {
        _board.OnTilesGenerated += SetupCamera;

        _inputManager.OnTouchStarted += DoOnStartTouch;

        _inputManager.OnPressed += MovementOnHoldTouch;

        _inputManager.OnTouchEnded += MovementOnEndTouch;

        _inputManager.OnTapDetected += DoOnTapDetected;
    }


    private void OnDisable()
    {
        _board.OnTilesGenerated -= SetupCamera;

        _inputManager.OnTouchStarted -= DoOnStartTouch;

        _inputManager.OnPressed -= MovementOnHoldTouch;

        _inputManager.OnTouchEnded -= MovementOnEndTouch;

        _inputManager.OnTapDetected -= DoOnTapDetected;
    }

#endregion
}