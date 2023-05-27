using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace DI.Infrastructure
{
    public class InputService : IInputService
    {
        public event Action<Vector3> OnTouchStarted;

        public event Action<Vector3> OnPressing;

        public event Action<Vector3> OnTapDetected;

        public event Action<Vector3> OnTouchEnded;

        private const float MinSwipeDistance = 0.5f;

        private const float MinTouchDuration = 0.2f;

        private readonly PlayerInput _input;

        private readonly Camera _inputCamera;

        private Vector3 _startTouchPosition;

        private Vector3 _endTouchPosition;

        private float _startTouchTime;

        private float _endTouchTime;


        public InputService(Camera inputCamera)
        {
            _input = new PlayerInput();
            _inputCamera = inputCamera;

            _input.Enable();

            _input.Touch.Press.started += StartTouch;
            _input.Touch.Press.canceled += EndTouch;
        }


        private void OnDestroy()
        {
            _input.Disable();

            _input.Touch.Press.started -= StartTouch;
            _input.Touch.Press.canceled -= EndTouch;
        }


        private void StartTouch(InputAction.CallbackContext context)
        {
            Vector2 touch = _input.Touch.TouchPosition.ReadValue<Vector2>();

            _startTouchPosition = GetWorldPosition(touch);

            _startTouchTime = (float)context.startTime;

            // StartCoroutine(HoldRoutine());

            OnTouchStarted?.Invoke(_startTouchPosition);
        }


        private IEnumerator HoldRoutine()
        {
            while (_input.Touch.Press.ReadValue<float>() != 0f)
            {
                Vector2 touch = _input.Touch.TouchPosition.ReadValue<Vector2>();

                Vector3 touchPos = GetWorldPosition(touch);

                OnPressing?.Invoke(touchPos);

                yield return null;
            }
        }


        private void EndTouch(InputAction.CallbackContext context)
        {
            Vector2 touch = _input.Touch.TouchPosition.ReadValue<Vector2>();

            _endTouchPosition = GetWorldPosition(touch);

            _endTouchTime = (float)context.time;

            float distance = Vector3.Distance(_startTouchPosition, _endTouchPosition);

            float touchDuration = _endTouchTime - _startTouchTime;

            if (touchDuration < MinTouchDuration &&
                distance < MinSwipeDistance)
            {
                OnTapDetected?.Invoke(_startTouchPosition);

                return;
            }

            OnTouchEnded?.Invoke(_endTouchPosition);
        }


        private Vector3 GetWorldPosition(Vector3 screenPosition)
        {
            Vector2 worldPos = _inputCamera.ScreenToWorldPoint(screenPosition);

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