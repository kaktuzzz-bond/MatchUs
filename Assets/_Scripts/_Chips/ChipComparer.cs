#define ENABLE_LOGS
using System;

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


    public Chip[] IsMatching(Chip chip)
    {
        //case: Storage is empty
        if (_storage == null)
        {
            PointerController.Instance.ShowPointer(PointerController.Selector, chip.BoardPosition);

            _storage = chip;

            return null;
        }

        //case: Tap the same
        if (chip.Equals(_storage))
        {
            Logger.Debug("The same first");

            _storage = null;

            PointerController.Instance.HidePointers();

            return null;
        }

        // case: Compare chips
        if (CompareChips(chip, _storage))
        {
            Chip other = _storage;

            _storage = null;

            PointerController.Instance.HidePointers();

            return new[] { chip, other };
        }

        PointerController.Instance.HidePointers();

        PointerController.Instance.ShowPointer(PointerController.Selector, chip.BoardPosition);

        _storage = chip;

        return null;
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