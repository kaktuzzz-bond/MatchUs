using Game;

namespace NonMono
{
    public static class ChipComparer
    {
        public static Chip _storage;


        public static void ClearStorage() => _storage = null;


        public static Chip[] HandleTap(Chip chip)
        {
            //case: Storage is empty
            if (_storage == null)
            {
                _storage = chip;

                return new[] { chip };
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

                return new[] { chip, other };
            }

            _storage = chip;

            return new[] { chip };
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
}