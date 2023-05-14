using Cysharp.Threading.Tasks;

public class FadedInChipState : IChipState
{
    public async UniTask Enter(Chip chip)
    {
        chip.Fade(1f);

        await UniTask.Yield();
    }
}