using System;
using Sirenix.OdinInspector;
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


    public void MakeDeselectable()
    {
        _button.interactable = false;
    }


    public void MakeSelectable()
    {
        _button.interactable = true;
    }


    public void Enable()
    {
        MakeSelectable();
        _button.enabled = true;
    }


    public void Disable()
    {
        MakeDeselectable();
        _button.enabled = false;
    }
}