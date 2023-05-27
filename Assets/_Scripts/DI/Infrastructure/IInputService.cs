using System;
using UnityEngine;

namespace DI.Infrastructure
{
    public interface IInputService
    {
        event Action<Vector3> OnTouchStarted;

        event Action<Vector3> OnPressing;

        event Action<Vector3> OnTapDetected;

        event Action<Vector3> OnTouchEnded;
    }
}