using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DI.Infrastructure
{
    public class VerticalMover : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        private IInputService _inputService;

        [SerializeField]
        private Vector3 _startTouchPosition;

        [SerializeField]
        private Vector3 _startTargetPosition;

        private CancellationTokenSource _cts;


        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;

            //_inputService.OnTouchStarted += StartTouch;
            _inputService.OnHoldTriggered += Hold;
            _inputService.OnTapTriggered += Tap;
            _inputService.OnTouchEnded += EndTouch;
        }


        private void Tap(Vector3 position)
        {
            Debug.Log($"Tap {position}");
        }


        private void Hold(Vector3 position)
        {
            Debug.Log($"Pressing {position}");

            _startTouchPosition = position;

            _startTargetPosition = target.position;

            MovingAsync().Forget();
        }


        async UniTaskVoid MovingAsync()
        {
            Vector3 offset = _startTargetPosition - _startTouchPosition;

            _cts = new CancellationTokenSource();

            Vector3 velocity = Vector3.zero;

            while (!_cts.Token.IsCancellationRequested)
            {
                Debug.LogWarning($"Current {_inputService.CurrentInputPosition} | {offset}");

                target.transform.position = Vector3.SmoothDamp(
                        target.transform.position,
                        _inputService.CurrentInputPosition + offset,
                        ref velocity,
                        0.1f);

                await UniTask.Yield();
            }
        }


        private void EndTouch(Vector3 position)
        {
            Debug.Log($"EndTouch {position}");

            _cts.Cancel();
        }


        private void StartTouch(Vector3 position)
        {
            Debug.Log($"Start touch {position}");
        }


        private void OnDisable()
        {
            _cts.Dispose();

            //_inputService.OnTouchStarted -= StartTouch;
            _inputService.OnHoldTriggered -= Hold;
            _inputService.OnTapTriggered -= Tap;
            _inputService.OnTouchEnded -= EndTouch;
        }
    }
}