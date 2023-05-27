using System;
using UnityEngine;

namespace DI.Infrastructure
{
    public interface IInputService
    {
        event Action<Vector3> OnTouchStarted;

        event Action<Vector3> OnHoldTriggered;

        event Action<Vector3> OnTapTriggered;

        event Action<Vector3> OnTouchEnded;


        Vector3 CurrentInputPosition { get; }
    }
}