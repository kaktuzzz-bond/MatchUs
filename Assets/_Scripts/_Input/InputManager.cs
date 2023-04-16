using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    #region Events

    public delegate void HoldCallback(Vector2 fingerPosition);

    public event HoldCallback OnHoldDetected;

    public delegate void EndTouchCallback(Vector2 startPosition, Vector2 endPosition);

    public event EndTouchCallback OnTouchEnded;

    #endregion

    private PlayerInput _input;

    private Vector2 _startTouchPosition;

    private bool _isHoldTriggered;


    private void Awake()
    {
        _input = new PlayerInput();
    }


    private void StartTouch(InputAction.CallbackContext context)
    {
        if (_isHoldTriggered) _isHoldTriggered = false;

        _startTouchPosition = _input.Touch.TouchPosition.ReadValue<Vector2>();
    }


    private void EndTouch(InputAction.CallbackContext context)
    {
        if (_isHoldTriggered) return;

        Vector2 endTouchPosition = _input.Touch.TouchPosition.ReadValue<Vector2>();

        OnTouchEnded?.Invoke(_startTouchPosition, endTouchPosition);
    }


    private void Hold(InputAction.CallbackContext context)
    {
        _isHoldTriggered = true;

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