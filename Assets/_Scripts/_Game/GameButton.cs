using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GameButton : MonoBehaviour
{
    private Button _button;


    private void Awake()
    {
        _button = GetComponent<Button>();
    }


    private void SetAbility(bool isEnabled)
    {
        _button.enabled = isEnabled;
    }


    public void SetInteractivity(bool isInteractable)
    {
        _button.interactable = isInteractable;
    }


#region ENABLE / DISABLE

    private void OnEnable()
    {
        GameGUI.Instance.OnButtonPressPermission += SetAbility;
    }


    private void OnDisable()
    {
        GameGUI.Instance.OnButtonPressPermission -= SetAbility;
    }

#endregion
}