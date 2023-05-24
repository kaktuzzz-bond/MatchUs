using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class GameButton : MonoBehaviour
    {
        private Button _button;

        private GameGUI _gameGUI;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _gameGUI = GameGUI.Instance;
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
            _gameGUI.OnButtonPressPermission += SetAbility;
        }


        private void OnDisable()
        {
            _gameGUI.OnButtonPressPermission -= SetAbility;
        }

    #endregion
    }
}