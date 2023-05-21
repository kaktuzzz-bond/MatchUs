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
        //_pointerController.OnPointersHidden += ClearStorage;
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
      
        
       
        if (CompareChips(chip, _storage))
        {
            Chip other = _storage;

            _storage = null;

            ChipController.Instance.PointerController.HidePointers();

            Board.Instance.ProcessMatched(chip, other).Forget();
            
            return;
        }

        ChipController.Instance.PointerController.HidePointers();

        ChipController.Instance.PointerController.ShowPointer(PointerController.Selector, chip.BoardPosition);

        _storage = chip;
    }


   
    public static bool CompareChips(Chip first, Chip second)
    {
        return (first.CompareHorizontalPosition(second) ||
                first.CompareVerticalPosition(second) ||
                first.CompareMultilinePosition(second)) &&
               (first.CompareShape(second) ||
                first.CompareColor(second));
    }


    ~ChipComparer()
    {
        _pointerController.OnPointersHidden -= ClearStorage;
    }
}