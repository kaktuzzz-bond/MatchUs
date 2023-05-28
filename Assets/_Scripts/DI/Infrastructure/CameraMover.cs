using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ModestTree;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace DI.Infrastructure
{
    [RequireComponent(typeof(Camera))]
    public class CameraMover : MonoBehaviour
    {
        [SerializeField]
        private float maxDistance;

        [SerializeField]
        private float moveDuration;

        private const float MovementMultiplier = 10f;
        
        private IInputService _inputService;

        private Camera _camera;

        private Vector3 _startPosition;

        private Vector3 _startTouchPosition;

        private Tween _tween;

        private CancellationTokenSource _cts;


        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;

            _inputService.OnTouchStarted += StartTouch;
            _inputService.OnHoldTriggered += Hold;
            _inputService.OnTapTriggered += Tap;
            _inputService.OnTouchEnded += EndTouch;

            _camera = GetComponent<Camera>();
        }


        private void StartTouch(Vector3 position)
        {
            Debug.Log($"Start touch {position}");

            _startTouchPosition = position;

            _startPosition = transform.position;

            _cts = new CancellationTokenSource();

            _tween?.Kill();
        }


        private void Tap(Vector3 position)
        {
            float distance = position.y - _startTouchPosition.y;

            if (Mathf.Abs(distance) > maxDistance)
            {
                Debug.LogWarning($"SWIPE {position}");

                MoveVertically(distance * MovementMultiplier, moveDuration);
                
                return;
            }

            Debug.Log($"Tap {position}");
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

            MoveVertically(_camera.velocity.y, moveDuration);
        }


        async UniTaskVoid FollowTouchPosition()
        {
            float offsetY = _startPosition.y - _startTouchPosition.y;

            float velocity = 0f;

            while (!_cts.Token.IsCancellationRequested)
            {
                Vector3 currentPos = transform.position;

                currentPos.y = Mathf.SmoothDamp(
                        currentPos.y,
                        _inputService.CurrentInputPosition.y + offsetY,
                        ref velocity,
                        0.1f);

                transform.position = currentPos;

                await UniTask.Yield();
            }
        }


        private void MoveVertically(float shift, float duration)
        {
            float targetY = transform.position.y + shift;

            _tween = transform.DOMoveY(targetY, duration)
                              .SetEase(Ease.OutCubic);
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