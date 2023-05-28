using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace DI.Infrastructure
{
    public class InputService : MonoBehaviour, IInputService
    {
        public event Action<Vector3> OnTouchStarted;

        public event Action<Vector3> OnHoldTriggered;

        public event Action<Vector3> OnTapTriggered;

        public event Action<Vector3> OnTouchEnded;

        public Vector3 CurrentInputPosition
        {
            get
            {
                Vector2 touch = _input.Touch.TouchPosition.ReadValue<Vector2>();

                return GetWorldPosition(touch);
            }
        }

        private PlayerInput _input;

        private Camera _camera;

        private Vector3 _startTouchPosition;

        private Vector3 _endTouchPosition;

        private float _startTouchTime;

        private float _endTouchTime;


        [Inject]
        private void Construct(CameraInputHandler inputCamera, Camera mainCamera)
        {
            _input = new PlayerInput();

            _camera = mainCamera;

            _input.Enable();

            _input.Touch.Press.started += StartTouch;
            _input.Touch.Press.canceled += EndTouch;
            _input.Touch.Hold.performed += Hold;
            _input.Touch.Tap.performed += Tap;
        }


        private void OnDisable()
        {
            _input.Disable();

            _input.Touch.Press.started -= StartTouch;
            _input.Touch.Press.canceled -= EndTouch;
            _input.Touch.Hold.performed -= Hold;
            _input.Touch.Tap.performed -= Tap;
        }


        private void Hold(InputAction.CallbackContext context)
        {
            Vector2 touch = _input.Touch.TouchPosition.ReadValue<Vector2>();

            Vector3 touchPosition = GetWorldPosition(touch);

            OnHoldTriggered?.Invoke(touchPosition);
        }


        private void Tap(InputAction.CallbackContext context)
        {
            Vector2 touch = _input.Touch.TouchPosition.ReadValue<Vector2>();

            Vector3 touchPosition = GetWorldPosition(touch);

            OnTapTriggered?.Invoke(touchPosition);
        }


        private void StartTouch(InputAction.CallbackContext context)
        {
            Vector2 touch = _input.Touch.TouchPosition.ReadValue<Vector2>();

            _startTouchPosition = GetWorldPosition(touch);

            _startTouchTime = (float)context.startTime;

            OnTouchStarted?.Invoke(_startTouchPosition);
        }


        private void EndTouch(InputAction.CallbackContext context)
        {
            Vector2 touch = _input.Touch.TouchPosition.ReadValue<Vector2>();

            _endTouchPosition = GetWorldPosition(touch);

            _endTouchTime = (float)context.time;

            float distance = Vector3.Distance(_startTouchPosition, _endTouchPosition);

            float touchDuration = _endTouchTime - _startTouchTime;

            OnTouchEnded?.Invoke(_endTouchPosition);
        }


        private Vector3 GetWorldPosition(Vector3 screenPosition)
        {
            Vector2 worldPos = _camera.ScreenToWorldPoint(screenPosition);

            return worldPos;
        }


        // private void DoSwipe(float distance)
        // {
        //     float duration = _endTouchTime - _startTouchTime;
        //
        //     float speed = distance / duration;
        //
        //     Vector3 direction = _endTouchPosition - _startTouchPosition;
        //
        //     Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
        //
        //     if (!(Vector2.Dot(Vector2.up, direction2D) > directionThreshold) &&
        //         !(Vector2.Dot(Vector2.down, direction2D) > directionThreshold))
        //     {
        //         return;
        //     }
        //
        //    // OnSwipeDetected?.Invoke(direction2D.y * speed, duration);
        //
        //     // we don't use horizontal gestures
        //     // else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        //     // {
        //     //     Debug.Log("Swipe Left");
        //     // }
        //     // else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        //     // {
        //     //     Debug.Log("Swipe Right");
        //     //}
        // }
    }
}