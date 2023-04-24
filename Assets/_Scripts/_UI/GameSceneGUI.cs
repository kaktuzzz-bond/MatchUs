using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneGUI : Singleton<GameSceneGUI>
{
    public event Action OnFadedIn;

    [SerializeField]
    private RectTransform header;

    [SerializeField]
    private Image fader;


    public Vector3[] GetHeaderCorners() => Utils.GetRectWorldCorners(header);


    private CameraController _cameraController;


    private void Awake()
    {
        _cameraController = CameraController.Instance;
    }


    private void FadeIn()
    {
        fader
                .DOFade(0, 0.5f)
                .SetEase(Ease.InSine)
                .onComplete += () => OnFadedIn?.Invoke();
    }


#region Enable / Disable

    private void OnEnable()
    {
        _cameraController.OnCameraSetup += FadeIn;
    }


    private void OnDisable()
    {
        _cameraController.OnCameraSetup -= FadeIn;
    }

#endregion
}