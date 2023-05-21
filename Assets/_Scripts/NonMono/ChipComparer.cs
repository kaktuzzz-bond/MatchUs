using System.Collections.Generic;
using UnityEngine;

public class ChipComparer
{
   
    private Chip _storage;
    public void ClearStorage() => _storage = null;
    
  
    public Chip[] HandleTap(Chip chip)
    {
        //case: Storage is empty
        if (_storage == null)
        {
            _storage = chip;

            return null;
        }

        //case: Tap the same
        if (chip.Equals(_storage))
        {
            _storage = null;

            return null;
        }

        // case: Compare chips
        if (CompareChips(chip, _storage))
        {
            Chip other = _storage;

            _storage = null;
            
            return new []{chip, other};
            
            
        }

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
    
}