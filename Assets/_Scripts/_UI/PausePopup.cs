using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour
{
    private const float PopDuration = 0.5f;

    [SerializeField]
    private Button close;

    [SerializeField]
    private Button info;

    [SerializeField]
    private Button home;

    [SerializeField]
    private Button restart;

    private GameGUI _gameGUI;


    private void Awake()
    {
        close.onClick.AddListener(() => ResumeGameAsync().Forget());

        info.onClick.AddListener(ShowInfo);

        home.onClick.AddListener(GoHome);

        restart.onClick.AddListener(Restart);

        _gameGUI = GameGUI.Instance;

        transform.localScale = Vector3.zero;
    }


    private async UniTask ResumeGameAsync()
    {
        // Debug.Log("RESUME");

        _gameGUI.SetButtonPressPermission(true);

        await HidePopupAsync();

        await _gameGUI.Fader.FadeOutEffect();

        GameManager.Instance.ResumeGame();
    }


    private void ShowInfo()
    {
        Debug.Log("SHOW INFO");
    }


    private void GoHome()
    {
        Debug.Log("MAIN SCREEN");
    }


    private void Restart()
    {
        Debug.Log("RESTART");
    }


    public async UniTask ShowPopupAsync()
    {
        await GameGUI.Instance.Fader.FadeInEffect();

        gameObject.SetActive(true);

        await transform.DOScale(Vector3.one, PopDuration)
                .SetEase(Ease.OutBack)
                .ToUniTask();

        Debug.Log("Show popup completed");
    }


    private async UniTask HidePopupAsync()
    {
        await transform.DOScale(Vector3.zero, PopDuration)
                .SetEase(Ease.InBack)
                .ToUniTask();

        await GameGUI.Instance.Fader.FadeOutEffect();

        gameObject.SetActive(false);
    }
}