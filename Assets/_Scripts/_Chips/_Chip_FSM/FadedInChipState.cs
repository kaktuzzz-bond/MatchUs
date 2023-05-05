public class FadedInChipState : IChipState
{
    public void Enter(Chip chip)
    {
        chip.Fade(1f);
    }
}