using DG.Tweening;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    private bool _isShown;

    private const float InfoJump = 2f;

    private const float InfoJumpDuration = 0.6f;

    private float _hiddenPositionY;

    private float _showedPositionY;


    public void Init()
    {
        _hiddenPositionY = transform.position.y;

        _showedPositionY = _hiddenPositionY + InfoJump;
    }


    public void Show()
    {
        if (_isShown) return;

        Debug.Log("Show Info");

        gameObject.SetActive(true);

        _isShown = true;

        transform
                .DOMoveY(_showedPositionY, InfoJumpDuration)
                .SetEase(Ease.OutBack);
    }


    public void Hide()
    {
        if (!_isShown) return;

        Debug.Log("Hide Info");

        transform
                .DOMoveY(_hiddenPositionY, InfoJumpDuration)
                .SetEase(Ease.InBack)
                .onComplete += () =>
        {
            gameObject.SetActive(false);
            _isShown = false;
        };
    }
}