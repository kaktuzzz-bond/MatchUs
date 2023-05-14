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

        //_button.interactable = isInteractable;
    }


    public void Enable()
    {
        _button.interactable = true;
    }


    public void Disable()
    {
        _button.interactable = false;
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