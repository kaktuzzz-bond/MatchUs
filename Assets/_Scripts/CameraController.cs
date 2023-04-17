using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

public class CameraController : Singleton<CameraController>
{
    [SerializeField, ReadOnly]
    private float durationMultiplier = 14f;

    public Camera MainCamera => _camera ??= Camera.main;

    private Tween _tween;


    private Camera _camera;


    public void Move(float yValue, float timeDuration)
    {
        MainCamera.transform.DOKill();

        MainCamera.transform
                .DOMoveY(MainCamera.transform.position.y - yValue, timeDuration * durationMultiplier)
                .SetEase(Ease.OutQuad);
    }
}