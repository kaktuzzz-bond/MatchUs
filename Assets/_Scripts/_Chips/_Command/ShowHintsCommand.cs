using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShowHintsCommand : ICommand
{
    private GamePointer[] _hinters;

    public event Action<Vector2Int, Vector2Int> OnHintFound;


    public void Execute()
    {
        var inGameChips = ChipController.Instance.ChipRegistry.ActiveChips;

        int count = inGameChips.Count;

        for (int i = 0; i < count - 1; i++)
        {
            for (int j = i + 1; j < count; j++)
            {
                Chip first = inGameChips[i];
                Chip second = inGameChips[j];

                if (!ChipComparer.CompareChips(first, second)) continue;

                OnHintFound?.Invoke(first.BoardPosition, second.BoardPosition);

                return;
            }
        }

        //Debug.Log("======== NO HINTS ========");

        GameGUI.Instance.ShowInfo();
    }


    public async UniTask Undo()
    {
        await UniTask.Yield();
    }
}