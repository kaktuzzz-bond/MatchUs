using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraShaker
{
    [Button("Shake")]
    public void Shake(Camera cam)
    {
        const float shakeDuration = 0.3f;

        Vector3 strength = new(0, 0, 2f);

        const int vibrato = 10;

        float startOrthoSize = cam.orthographicSize;

        float zoomed = startOrthoSize * 0.9f;

        cam
                .DOShakeRotation(shakeDuration, strength, vibrato);

        cam
                .DOOrthoSize(zoomed, shakeDuration * 0.5f)
                .onComplete += () =>
                cam
                        .DOOrthoSize(startOrthoSize, shakeDuration * 0.5f);
    }
}