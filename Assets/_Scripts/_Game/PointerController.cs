using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class PointerController : Singleton<PointerController>
{
    public event Action OnPointersHidden;

    public const string Selector = "Selector";

    private const string Hint = "Hint";

    [SerializeField] [FoldoutGroup("Prefabs")]
    private Transform selectorPrefab;

    [SerializeField] [FoldoutGroup("Prefabs")]
    private Transform hintPrefab;

    [ShowInInspector]
    private Dictionary<string, ObjectPool> _pools;

    private Board _board;

    private bool _isHintShown;


    private void Awake()
    {
        _board = Board.Instance;
    }


    private void Start()
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
                .SetPosition(_board[boardPosition.x, boardPosition.y].position)
                .SetParent(_board.pointerParent)
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