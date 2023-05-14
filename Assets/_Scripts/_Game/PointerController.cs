using System;
using System.Collections.Generic;
using UnityEngine;

public class PointerController
{
    public event Action OnPointersHidden;

    public const string Selector = "Selector";

    private const string Hint = "Hint";

    private Dictionary<string, ObjectPool> _pools;

    private bool _isHintShown;


    public PointerController(Transform selectorPrefab, Transform hintPrefab)
    {
        _pools = new Dictionary<string, ObjectPool>
        {
                { selectorPrefab.tag, new ObjectPool(selectorPrefab) },
                { hintPrefab.tag, new ObjectPool(hintPrefab) }
        };
    }


    public void ShowHints()
    {
        HidePointers();

        ShowHintsCommand hintsCommand = new();

        hintsCommand.OnHintFound += (first, second) =>
        {
            ShowPointer(Hint, first);
            ShowPointer(Hint, second);

            CameraController.Instance.MoveToBoardPosition(Mathf.Max(first.y, second.y));

            _isHintShown = true;
        };

        hintsCommand.Execute();
    }


    public void ShowPointer(string pointerTag, Vector2Int boardPosition)
    {
        _pools[pointerTag]
                .Get()
                .GetComponent<GamePointer>()
                .SetName()
                .SetPosition(Board.Instance[boardPosition.x, boardPosition.y].position)
                .SetParent(Board.Instance.pointerParent)
                .Subscribe()
                .Show();
    }


    public void ReleasePointer(GamePointer gamePointer)
    {
        _pools[gamePointer.transform.tag].Release(gamePointer.transform);
    }


    public void HidePointers()
    {
        OnPointersHidden?.Invoke();
    }


    public void CheckForHints()
    {
        if (!_isHintShown) return;

        HidePointers();
        _isHintShown = false;
    }
}