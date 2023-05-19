using System.Collections.Generic;
using UnityEngine;

public class ChipComparer
{
    private Chip _storage;

    private PointerController _pointerController;


    public void ClearStorage() => _storage = null;


    public ChipComparer(PointerController pointerController)
    {
        _pointerController = pointerController;
        _pointerController.OnPointersHidden += ClearStorage;
    }


    public void TryMatching(Chip chip)
    {
        //case: Storage is empty
        if (_storage == null)
        {
            ChipController.Instance.PointerController.ShowPointer(PointerController.Selector, chip.BoardPosition);

            _storage = chip;

            return;
        }

        //case: Tap the same
        if (chip.Equals(_storage))
        {
            Debug.Log("The same first");

            _storage = null;

            ChipController.Instance.PointerController.HidePointers();

            return;
        }

        // case: Compare chips
        var compareResult = CompareChips(chip, _storage);
       
        if (compareResult != null)
        {
            Chip other = _storage;

            _storage = null;

            ChipController.Instance.PointerController.HidePointers();

            Board.Instance.ProcessMatched(chip, other, compareResult).Forget();
            
            return;
        }

        ChipController.Instance.PointerController.HidePointers();

        ChipController.Instance.PointerController.ShowPointer(PointerController.Selector, chip.BoardPosition);

        _storage = chip;
    }


    public static List<Vector3[]> CompareChips(Chip first, Chip second)
    {
        if (!first.CompareShape(second) &&
            !first.CompareColor(second))
        {
            return null;
        }

        var horizontal = first.CompareHorizontalPosition(second);

        if (horizontal != null) return new List<Vector3[]>() { horizontal };

        var vertical = first.CompareVerticalPosition(second);

        if (vertical != null) return new List<Vector3[]>() { vertical };

        var multiline = first.CompareMultilinePosition(second);

        return multiline;
    }


    ~ChipComparer()
    {
        _pointerController.OnPointersHidden -= ClearStorage;
    }
}