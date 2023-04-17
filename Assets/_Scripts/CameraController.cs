using System;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraController : Singleton<CameraController>
{
    [SerializeField, ReadOnly]
    private float durationMultiplier = 14f;

    private Camera _camera;

    private Board _board;


    private void Awake()
    {
        _board = Board.Instance;
        _camera = Camera.main;
    }


    private void Setup()
    {
        SetOrthographicSize();
        SetInitialPosition();
    }


    private void SetOrthographicSize() =>
            _camera.orthographicSize = (_board.Width + 1f) * Screen.height / Screen.width * 0.5f;


    private void SetInitialPosition() =>
            _camera.transform.position = new Vector3(
                    (_board.Width - 1f) * 0.5f,
                    0f,
                    _camera.nearClipPlane);


    public void Move(float yValue, float timeDuration)
    {
        _camera.transform.DOKill();

        _camera.transform
                .DOMoveY(_camera.transform.position.y - yValue, timeDuration * durationMultiplier)
                .SetEase(Ease.OutQuad);
    }


    #region Enable / Disable

    private void OnEnable()
    {
        Board.Instance.OnTilesGenerated += Setup;
    }


    private void OnDisable()
    {
        Board.Instance.OnTilesGenerated -= Setup;
    }

    #endregion
}