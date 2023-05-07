#define ENABLE_LOGS
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ChipComparer : Singleton<ChipComparer>
{
    public event Action<Chip, Chip> OnChipMatched;

    [ShowInInspector]
    private Chip _storage;

    private PointerController _pointerController;

    private CameraController _cameraController;


    public void ClearStorage() => _storage = null;


    private void Awake()
    {
        _pointerController = PointerController.Instance;
        _cameraController = CameraController.Instance;
    }


    private void ProcessChip(Chip chip)
    {
        //case: Storage is empty
        if (_storage == null)
        {
            _pointerController.ShowPointer(PointerController.Selector, chip.BoardPosition);

            _storage = chip;

            return;
        }

        //case: Tap the same
        if (chip.Equals(_storage))
        {
            Logger.Debug("The same first");

            _storage = null;

            _pointerController.HidePointers();

            return;
        }

        // case: Compare chips

        if (CompareChips(chip, _storage))
        {
            OnChipMatched?.Invoke(chip, _storage);

            _storage = null;

            _pointerController.HidePointers();
        }
        else
        {
            _pointerController.HidePointers();

            _pointerController.ShowPointer(PointerController.Selector, chip.BoardPosition);

            _storage = chip;
        }
    }


    public static bool CompareChips(Chip first, Chip second)
    {
        return (first.CompareHorizontalPosition(second) ||
               first.CompareVerticalPosition(second) ||
               first.CompareMultilinePosition(second)) &&
               (first.CompareShape(second) ||
               first.CompareColor(second));
    }


#region Enable / Disable

    private void OnEnable()
    {
        _cameraController.OnChipTapped += ProcessChip;
    }


    private void OnDisable()
    {
        _cameraController.OnChipTapped -= ProcessChip;
    }

#endregion
}