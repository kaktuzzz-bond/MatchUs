using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class InputHandler : Singleton<InputHandler>
{
    [HorizontalGroup("Split", Title = "Swipe Properties", Width = 0.5f)]
    [SerializeField, BoxGroup("Split/Min Swipe Distance"), HideLabel]
    private float minSwipeDistance = 0.5f;

    [SerializeField, PropertyRange(0f, 1f), BoxGroup("Split/Direction Threshold"), HideLabel]
    private float directionThreshold = 0.8f;

    private InputManager _inputManager;

    private Camera _camera;


    private void Awake()
    {
        _inputManager = InputManager.Instance;

        _camera = CameraController.Instance.MainCamera;
    }

    private void StartTouch(Vector2 fingerPosition)
    {
        Vector3 fingerPos = Utils.ScreenToWorldPosition(_camera, fingerPosition);

        Debug.Log($"{nameof(StartTouch)} in {fingerPos}");
    }

    private void Tap(Vector3 worldFingerPosition)
    {
        Debug.Log($"{nameof(Tap)} in {worldFingerPosition}");
    }


    private void Hold(Vector2 fingerPosition)
    {
        Vector3 fingerPos = Utils.ScreenToWorldPosition(_camera, fingerPosition);

        Debug.Log($"{nameof(Hold)} in {fingerPos}");
    }


    private void Swipe(Vector2 startPosition, Vector2 endPosition, float timeDuration)
    {
        Vector3 startPos = Utils.ScreenToWorldPosition(_camera, startPosition);

        Vector3 endPos = Utils.ScreenToWorldPosition(_camera, endPosition);

        float distance = Vector3.Distance(startPos, endPos);

        float speed = distance / timeDuration;

        Debug.LogWarning(timeDuration + " | " + speed);

        if (distance < minSwipeDistance)
        {
            Tap(startPos);

            return;
        }

        Debug.DrawLine(startPos, endPos, Color.red, 3f);

        Vector3 direction = endPos - startPos;

        Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;

        SwipeDirection(direction2D, speed, timeDuration);
    }


    private void SwipeDirection(Vector2 direction, float speed, float timeDuration)
    {
        if (!(Vector2.Dot(Vector2.up, direction) > directionThreshold) &&
            !(Vector2.Dot(Vector2.down, direction) > directionThreshold))
        {
            return;
        }

        Debug.Log("Swipe " + direction + " | " + speed);
            
        CameraController.Instance.Move(direction.y * speed, timeDuration);
        
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


    #region Enable / Disable

    private void OnEnable()
    {
        _inputManager.OnTouchStarted += StartTouch;
        _inputManager.OnHoldDetected += Hold;
        _inputManager.OnTouchEnded += Swipe;
    }


    private void OnDisable()
    {
        _inputManager.OnTouchStarted -= StartTouch;
        _inputManager.OnHoldDetected -= Hold;
        _inputManager.OnTouchEnded -= Swipe;
    }

    #endregion
}