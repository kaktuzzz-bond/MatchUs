using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    #region Events

    public delegate void ScreenTouchedCallback(Vector3 tapPosition);

    public event ScreenTouchedCallback OnTouchStarted;

    public event ScreenTouchedCallback OnTapDetected;

    public delegate void SwipeDetectedCallback(float tweenEndValue, float tweenDuration);

    public event SwipeDetectedCallback OnSwipeDetected;

    public delegate void HoldCallback(Vector2 offset);

    public event HoldCallback OnHoldPressed;

    #endregion

    [HorizontalGroup("Split", Title = "Swipe Properties", Width = 0.5f)]
    [SerializeField, BoxGroup("Split/Min Swipe Distance"), HideLabel]
    private float minSwipeDistance = 0.5f;

    [SerializeField, PropertyRange(0f, 1f), BoxGroup("Split/Direction Threshold"), HideLabel]
    private float directionThreshold = 0.8f;

    private PlayerInput _input;

    private Camera _camera;

    private Vector3 _startTouchPosition;

    private Vector3 _endTouchPosition;

    private float _startTouchTime;

    private float _endTouchTime;


    private void Awake()
    {
        _input = new PlayerInput();

        _camera = Camera.main;
    }


    private void StartTouch(InputAction.CallbackContext context)
    {
        Vector2 touch = _input.Touch.TouchPosition.ReadValue<Vector2>();

        _startTouchPosition = Utils.ScreenToWorldPosition(_camera, touch);

        _startTouchTime = (float)context.startTime;

        OnTouchStarted?.Invoke(_startTouchPosition);
    }


    private void Hold(InputAction.CallbackContext context)
    {
        StartCoroutine(HoldRoutine());
    }


    private IEnumerator HoldRoutine()
    {
        while (_input.Touch.Press.ReadValue<float>() != 0f)
        {
            Vector2 touch = _input.Touch.TouchPosition.ReadValue<Vector2>();

            Vector3 touchPos = Utils.ScreenToWorldPosition(_camera, touch);

            Vector2 holdOffset = touchPos - _startTouchPosition;

            OnHoldPressed?.Invoke(holdOffset);

            yield return null;
        }
    }


    private void EndTouch(InputAction.CallbackContext context)
    {
        Vector2 touch = _input.Touch.TouchPosition.ReadValue<Vector2>();

        _endTouchPosition = Utils.ScreenToWorldPosition(_camera, touch);

        _endTouchTime = (float)context.time;

        float distance = Vector3.Distance(_startTouchPosition, _endTouchPosition);

        if (CheckForTap(distance))
        {
            return;
        }

        DoSwipe(distance);
    }


    private bool CheckForTap(float distance)
    {
        if (distance > minSwipeDistance) return false;

        OnTapDetected?.Invoke(_startTouchPosition);

        return true;
    }


    private void DoSwipe(float distance)
    {
        float duration = _endTouchTime - _startTouchTime;

        float speed = distance / duration;

        Vector3 direction = _endTouchPosition - _startTouchPosition;

        Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;

        if (!(Vector2.Dot(Vector2.up, direction2D) > directionThreshold) &&
            !(Vector2.Dot(Vector2.down, direction2D) > directionThreshold))
        {
            return;
        }

        OnSwipeDetected?.Invoke(direction2D.y * speed, duration);

        // we don't use horizontal gestures
        // else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        // {
        //     Debug.Log("Swipe Left");
        // }
        // else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        // {
        //     Debug.Log("Swipe Right");
        //}
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