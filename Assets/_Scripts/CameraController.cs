using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraController : Singleton<CameraController>
{
    [SerializeField, ReadOnly]
    private float durationMultiplier = 14f;

    public Camera MainCamera { get; private set; }

    private Tween _tween;


    private void Awake()
    {
        MainCamera = Camera.main;
        if (MainCamera) MainCamera.nearClipPlane = 0f;
    }


    public void Move(float yValue, float timeDuration)
    {
        MainCamera.transform.DOKill();

        MainCamera.transform
                .DOMoveY(MainCamera.transform.position.y - yValue, timeDuration * durationMultiplier)
                .SetEase(Ease.OutQuad);
    }
}