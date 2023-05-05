public class FadedOutChipState : IChipState
{
    public void Enter(Chip chip)
    {
        chip.Fade(0.12f);
    }
}