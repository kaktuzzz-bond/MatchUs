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


    private void CompareStorage(Chip chip)
    {
        bool isInPosition = chip.CompareHorizontalPosition(_storage) ||
                            chip.CompareVerticalPosition(_storage) ||
                            chip.CompareMultilinePosition(_storage);

        bool isComparing = chip.CompareShape(_storage) ||
                           chip.CompareColor(_storage);

        if (isInPosition && isComparing)
        {
            OnChipMatched?.Invoke(chip, _storage);

            _storage = null;

            _pointerController.HidePointers();
        }
        else
        {
            _pointerController.HidePointers();

            _pointerController.GetPointer(PointerController.Selector, chip.BoardPosition);

            _storage = chip;
        }
    }


    private void ProcessChip(Chip chip)
    {
        //case: Storage is empty
        if (_storage == null)
        {
            _pointerController.GetPointer(PointerController.Selector, chip.BoardPosition);

            _storage = chip;

            return;
        }

        //case: Tap the same
        if (chip.Equals(_storage))
        {
            Logger.Debug("The same chip");

            _storage = null;

            _pointerController.HidePointers();

            return;
        }

        // case: Compare chips
        CompareStorage(chip);
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