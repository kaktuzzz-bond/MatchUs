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


    [Button("Disable")]
    public void Disable()
    {
        _button.interactable = false;
    }


    [Button("Enable")]
    public void Enable()
    {
        _button.interactable = true;
    }
}