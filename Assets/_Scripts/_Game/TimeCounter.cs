
using UnityEngine;

public class TimeCounter : Singleton<TimeCounter>
{
    private float _timerCounter;

    private bool _timerOn;


    private void Update()
    {
        CountTime();
    }


    private void CountTime()
    {
        if (!_timerOn) return;

        _timerCounter += Time.deltaTime;

        GameGUI.Instance.UpdateTime(_timerCounter);
    }

    private void StopCount()
    {
        _timerOn = false;
    }


    private void ResumeCount()
    {
        _timerOn = true;
    }


    private void PauseCount()
    {
        _timerOn = false;
    }


    private void StartCount()
    {
        _timerCounter = 0;
        _timerOn = true;
    }


#region ENABLE / DISABLE

    private void OnEnable()
    {
        GameManager.Instance.OnGameStarted += StartCount;
        GameManager.Instance.OnGamePaused += PauseCount;
        GameManager.Instance.OnGameResumed += ResumeCount;
        GameManager.Instance.OnGameOver += StopCount;
    }


  

    private void OnDisable()
    {
        GameManager.Instance.OnGameStarted -= StartCount;
        GameManager.Instance.OnGamePaused -= PauseCount;
        GameManager.Instance.OnGameResumed -= ResumeCount;
        GameManager.Instance.OnGameOver -= StopCount;
    }

#endregion
}