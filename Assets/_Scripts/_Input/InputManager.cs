using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    #region Events

    public delegate void TouchPointCallback(Vector2 fingerPosition);

    public event TouchPointCallback OnHoldDetected;

    public event TouchPointCallback OnTouchStarted;
    public delegate void EndTouchCallback(Vector2 startPosition, Vector2 endPosition, float timeDuration);

    public event EndTouchCallback OnTouchEnded;

    #endregion

    private PlayerInput _input;

    private Vector2 _startTouchPosition;

    private float _startTime;

    //private bool _isHoldTriggered;


    private void Awake()
    {
        _input = new PlayerInput();
    }


    private void StartTouch(InputAction.CallbackContext context)
    {
        //if (_isHoldTriggered) _isHoldTriggered = false;

        _startTouchPosition = _input.Touch.TouchPosition.ReadValue<Vector2>();
        _startTime = (float)context.startTime;
        
        OnTouchStarted?.Invoke(_startTouchPosition);
    }


    private void EndTouch(InputAction.CallbackContext context)
    {
        //if (_isHoldTriggered) return;

        OnTouchEnded?.Invoke(
                startPosition: _startTouchPosition,
                endPosition: _input.Touch.TouchPosition.ReadValue<Vector2>(),
                timeDuration: (float)context.time - _startTime);
    }


    private void Hold(InputAction.CallbackContext context)
    {
        //_isHoldTriggered = true;

        StartCoroutine(HoldRoutine());
    }


    private IEnumerator HoldRoutine()
    {
        while (_input.Touch.Press.ReadValue<float>() != 0f)
        {
            Vector2 touchPosition = _input.Touch.TouchPosition.ReadValue<Vector2>();

            OnHoldDetected?.Invoke(touchPosition);

            yield return null;
        }
    }


    #region Enabel / Disable

    private void OnEnable()
    {
        _input.Enable();

        _input.Touch.Press.started += StartTouch;
        _input.Touch.Press.canceled += EndTouch;
        _input.Touch.Hold.performed += Hold;
    }


    private void OnDisable()
    {
        _input.Touch.Press.started -= StartTouch;
        _input.Touch.Press.canceled -= EndTouch;
        _input.Touch.Hold.performed -= Hold;

        _input.Disable();
    }

    #endregion
}