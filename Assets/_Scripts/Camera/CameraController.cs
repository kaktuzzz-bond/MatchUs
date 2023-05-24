using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game;
using NonMono;
using UnityEngine;
using Sirenix.OdinInspector;
using UI;

public class CameraController : Singleton<CameraController>
{
    //public event Action OnCameraSetup;

    [SerializeField, MinValue(0)]
    private float cameraVelocityThreshold = 1f;

    [SerializeField, MinValue(0)]
    private float endTouchMoveDuration = 1f;

    [SerializeField] [MinValue(0)]
    private float onHoldMoveDuration = 0.2f;

    [SerializeField, MinValue(0)]
    private float distanceToFooter = 1f;

    [SerializeField, MinValue(0)]
    private float moveToBottomBoundDuration = 1f;

    private GameManager _gameManager;
    
    private Camera _camera;

    private InputManager _inputManager;

    private GameGUI _gameGUI;

    private Vector3 _startCameraPosition;

    private Vector3 _startTouchFingerPosition;

    private bool _isStopMovement;

    // Limits of camera movement

    private Vector3 _topBoundPoint;

    private Vector3 _bottomBoundPoint;

    private float _camToNextPositionDistance;

#region INITIALIZATION

    private void Awake()
    {
       

        _inputManager = InputManager.Instance;

        _gameGUI = GameGUI.Instance;

        _gameManager = GameManager.Instance;

        _camera = Camera.main;
    }

#endregion

#region PLAYER INPUT ACTIONS

    private void DoOnStartTouch(Vector3 position)
    {
        //ChipController.Instance.PointerController.CheckForHints();

        //TODO uncomment if it's necessary 
        //GameGUI.Instance.HideInfo();

        _isStopMovement = Mathf.Abs(_camera.velocity.y) > cameraVelocityThreshold;

        _camera.transform.DOKill();

        _startCameraPosition = _camera.transform.position;

        _startTouchFingerPosition = position;
    }


    private void DoOnTapDetected(Vector3 position)
    {
        if (_isStopMovement) return;

        if (!_gameGUI.IsGameAreaPosition(position)) return;

        RaycastHit2D hit = Physics2D.Raycast(position, -Vector2.up);

        if (hit.collider == null) return;

        if (!hit.collider.TryGetComponent(out Chip chip)) return;

        bool isChipFadedIn = chip.CurrentChipState == ChipState.LightOn;

        if (isChipFadedIn)
        {
            ChipController.Instance.ProcessTappedChip(chip).Forget();
        }
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

        LimitCameraMovementToBounds(targetValue);
    }


    public void MoveToBottomBound()
    {
        CalculateBottomBound();

        _camera.transform
                .DOMoveY(_bottomBoundPoint.y, moveToBottomBoundDuration)
                .SetEase(Ease.OutQuad);
    }


    public void MoveToBoardPosition(int boardLine)
    {
        float targetY = _gameManager.gameData.Board[0, boardLine].y;

        targetY = Mathf.Clamp(targetY, _bottomBoundPoint.y, _topBoundPoint.y);

        _camera.transform
                .DOMoveY(targetY, moveToBottomBoundDuration)
                .SetEase(Ease.OutQuad);
    }

#endregion

#region CAMERA SETUP

    private void LimitCameraMovementToBounds(float targetValue)
    {
        if (targetValue > _topBoundPoint.y)
        {
            _camera.transform
                    .DOMoveY(_topBoundPoint.y, onHoldMoveDuration)
                    .SetEase(Ease.Linear);
        }
        else if (targetValue < _bottomBoundPoint.y)
        {
            _camera.transform
                    .DOMoveY(_bottomBoundPoint.y, onHoldMoveDuration)
                    .SetEase(Ease.Linear);
        }
        else
        {
            _camera.transform
                    .DOMoveY(targetValue, endTouchMoveDuration)
                    .SetEase(Ease.OutQuad);
        }
    }

    public async UniTask SetOrthographicSizeAsync()
    {
        _camera.orthographicSize = _gameManager.gameData.CameraOrthographicSize;
    
        await UniTask.WaitForEndOfFrame(this);
    }


    public async UniTask SetBoundsAsync()
    {
        var rectHeader = _gameGUI.GetHeaderCorners();

        float headerHeight = rectHeader[1].y - rectHeader[0].y;

        _topBoundPoint = new Vector3(
                x: (_gameManager.gameData.width - 1f) * 0.5f,
                y: headerHeight - _camera.orthographicSize + 0.5f,
                z: _camera.transform.position.z);

        // supposed distance between camera and next chip position
        var rectFooter = _gameGUI.GetFooterCorners();

        float footerHeight = rectFooter[1].y - rectFooter[0].y;

        _camToNextPositionDistance = _camera.orthographicSize - footerHeight - distanceToFooter;

        _bottomBoundPoint = _topBoundPoint;

        CalculateBottomBound();

        await UniTask.Yield();
    }


    private void CalculateBottomBound()
    {
        Vector2Int nextBoardPos = ChipInfo.GetBoardPosition(ChipRegistry.Counter);

        Vector3 worldPos = _gameManager.gameData.Board[nextBoardPos.x, nextBoardPos.y];

        _bottomBoundPoint.y = worldPos.y + _camToNextPositionDistance;

        if (_bottomBoundPoint.y > _topBoundPoint.y)
        {
            _bottomBoundPoint.y = _topBoundPoint.y;
        }
    }


    public async UniTask SetInitialPositionAsync()
    {
        _camera.transform.position = _bottomBoundPoint;

        await UniTask.Yield();
    }

#endregion

#region ENABLE / DISABLE

    private void OnEnable()
    {
        _inputManager.OnTouchStarted += DoOnStartTouch;

        _inputManager.OnPressed += MovementOnHoldTouch;

        _inputManager.OnTouchEnded += MovementOnEndTouch;

        _inputManager.OnTapDetected += DoOnTapDetected;
    }


    private void OnDisable()
    {
        _inputManager.OnTouchStarted -= DoOnStartTouch;

        _inputManager.OnPressed -= MovementOnHoldTouch;

        _inputManager.OnTouchEnded -= MovementOnEndTouch;

        _inputManager.OnTapDetected -= DoOnTapDetected;
    }

#endregion
}