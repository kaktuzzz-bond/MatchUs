using System;
using UnityEngine;

namespace DI.Infrastructure
{
    public class TestInputCatcher : MonoBehaviour
    {
        private IInputService _inputService;


        private void OnEnable()
        {
            _inputService.OnTouchStarted += StartTouch;
            _inputService.OnPressing += Pressing;
            _inputService.OnTapDetected += DetectTap;
            _inputService.OnTouchEnded += EndTouch;
        }


        private void OnDisable()
        {
            _inputService.OnTouchStarted -= StartTouch;
            _inputService.OnPressing -= Pressing;
            _inputService.OnTapDetected -= DetectTap;
            _inputService.OnTouchEnded -= EndTouch;
        }


        private void EndTouch(Vector3 pos)
        {
            Debug.Log($"EndTouch {pos}");
        }


        private void DetectTap(Vector3 pos)
        {
            Debug.Log($"Tap {pos}");
        }


        private void Pressing(Vector3 pos)
        {
            Debug.Log($"Pressing {pos}");
        }


        private void StartTouch(Vector3 pos)
        {
            Debug.Log($"Start touch {pos}");
        }
    }
}