using System;
using System.Collections.Generic;
using UnityEngine;

public class PointerPool
{
    public static  string selector = "Selector";

    public static  string hint = "Hint";

    private readonly Dictionary<string, ObjectPool<GamePointer>> _pools;
    


    public PointerPool(Transform selectorPrefab, Transform hintPrefab)
    {
        selector = selectorPrefab.tag;

        hint = hintPrefab.tag;
        
        _pools = new Dictionary<string, ObjectPool<GamePointer>>
        {
                { selector, new ObjectPool<GamePointer>(selectorPrefab.GetComponent<GamePointer>()) },
                { hint, new ObjectPool<GamePointer>(hintPrefab.GetComponent<GamePointer>()) }
        };
    }


    public void ShowHints()
    {
        ShowHintsCommand hintsCommand = new();

        hintsCommand.OnHintFound += (first, second) =>
        {
            ShowPointer(hint, first);
            ShowPointer(hint, second);

            CameraController.Instance.MoveToBoardPosition(Mathf.Max(first.y, second.y));
        };

        hintsCommand.Execute();
    }


    public void ShowPointer(string pointerTag, Vector2Int boardPosition)
    {
        _pools[pointerTag]
                .Get()
                .SetName(pointerTag)
                .SetPosition(Board.Instance[boardPosition.x, boardPosition.y])
                .SetParent(GameManager.Instance.gameData.pointerParent)
                .Show();
    }


    public void ReleasePointer(GamePointer gamePointer)
    {
        _pools[gamePointer.transform.tag].Release(gamePointer);
    }
}