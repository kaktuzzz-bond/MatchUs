using Cysharp.Threading.Tasks;

public class LightedOffChipState : IChipState
{
    public void Enter(Chip chip)
    {
        chip.Activate(true);

        LightOffAsync(chip).Forget();
    }


    private async UniTaskVoid LightOffAsync(Chip chip)
    {
        await chip.Fade(0.12f);
    }
}