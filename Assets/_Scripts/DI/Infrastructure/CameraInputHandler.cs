using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace DI.Infrastructure
{
    public class CameraInputHandler : MonoBehaviour
    {
        public event Action<Vector3> OnClearTapDetected;

        private Camera _camera;

        [SerializeField]
        private float maxDistance;

        [SerializeField]
        private float moveDuration;

        private const float MovementMultiplier = 10f;

        private IInputService _inputService;

        private Vector3 _startCameraPosition;

        private Vector3 _startTouchPosition;

        private float _startOffset;

        private Tween _tween;

        private CancellationTokenSource _cts;


        [Inject]
        private void Construct(IInputService inputService, Camera mainCamera)
        {
            _inputService = inputService;
            _camera = mainCamera;

            _inputService.OnTouchStarted += StartTouch;
            _inputService.OnHoldTriggered += Hold;
            _inputService.OnTapTriggered += Tap;
            _inputService.OnTouchEnded += EndTouch;
        }


        private void StartTouch(Vector3 position)
        {
            Debug.Log($"Start touch {position}");

            _startTouchPosition = position;

            _startCameraPosition = _camera.transform.position;

            _startOffset = _startCameraPosition.y - _startTouchPosition.y;

            _cts = new CancellationTokenSource();

            _tween?.Kill();
        }


        private void Tap(Vector3 position)
        {
            float distance = position.y - _startTouchPosition.y;

            if (Mathf.Abs(distance) > maxDistance)
            {
                MoveVertically(_camera.velocity.y);

                return;
            }

            OnClearTapDetected?.Invoke(position);
        }


        private void Hold(Vector3 position)
        {
            Debug.Log($"Pressing {position}");

            FollowTouchPosition().Forget();
        }


        private void EndTouch(Vector3 position)
        {
            _cts?.Cancel();

            Debug.Log($"End touch shift = ({_camera.velocity.y})");

            Debug.DrawRay(_startTouchPosition, Vector3.right, Color.green, 3f);

            Debug.DrawLine(_startTouchPosition, position, Color.red, 3f);

            Debug.LogWarning($"Velocity ({_camera.velocity})");

            MoveVertically(_camera.velocity.y);
        }


        async UniTaskVoid FollowTouchPosition()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                float currentOffset = _inputService.CurrentInputPosition.y - _startTouchPosition.y;

                float targetValue = _startCameraPosition.y - currentOffset;

                _camera.transform
                       .DOMoveY(targetValue, 0.1f);

                await UniTask.Yield();
            }
        }


        private void MoveVertically(float shift)
        {
            float targetY = _camera.transform.position.y + shift;

            _tween = _camera.transform.DOMoveY(targetY, moveDuration)
                            .SetEase(Ease.OutQuart);
        }


        private void OnDisable()
        {
            _cts?.Dispose();

            _inputService.OnTouchStarted -= StartTouch;
            _inputService.OnHoldTriggered -= Hold;
            _inputService.OnTapTriggered -= Tap;
            _inputService.OnTouchEnded -= EndTouch;
        }
    }
}