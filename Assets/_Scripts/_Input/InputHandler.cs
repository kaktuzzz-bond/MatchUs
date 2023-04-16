using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class InputHandler : Singleton<InputHandler>
{
    [SerializeField]
    private float minSwipeDistance = 0.5f;

    [SerializeField, PropertyRange(0f,1f)]
    private float directionThreshold = 0.9f;

    private InputManager _inputManager;

    private Camera _camera;


    private void Awake()
    {
        _inputManager = InputManager.Instance;
        _camera = Camera.main;
    }


    private void Tap(Vector3 worldFingerPosition)
    {
        Debug.Log($"{nameof(Tap)} touch in {worldFingerPosition}");
    }


    private void Hold(Vector2 fingerPosition)
    {
        Vector3 fingerPos = Utils.ScreenToWorldPosition(_camera, fingerPosition);

        Debug.Log($"{nameof(Hold)} touch in {fingerPos}");
    }


    private void Swipe(Vector2 startPosition, Vector2 endPosition)
    {
        Vector3 startPos = Utils.ScreenToWorldPosition(_camera, startPosition);

        Vector3 endPos = Utils.ScreenToWorldPosition(_camera, endPosition);

        float distance = Vector3.Distance(startPos, endPos);

        if (distance < minSwipeDistance)
        {
            Tap(startPosition);

            return;
        }

        Debug.DrawLine(startPos, endPos, Color.red, 3f);

        Vector3 direction = endPos - startPos;

        Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;

        SwipeDirection(direction2D);
    }


    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("Swipe Up");
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Swipe Down");
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Swipe Left");
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Swipe Right");
        }
    }


    #region Enable / Disable

    private void OnEnable()
    {
        _inputManager.OnHoldDetected += Hold;
        _inputManager.OnTouchEnded += Swipe;
    }


    private void OnDisable()
    {
        _inputManager.OnHoldDetected -= Hold;
        _inputManager.OnTouchEnded -= Swipe;
    }

    #endregion
}