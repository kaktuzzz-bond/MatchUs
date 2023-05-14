using Cysharp.Threading.Tasks;

public class FadedOutChipState : IChipState
{
    public async UniTask Enter(Chip chip)
    {
        chip.Fade(0.12f);

        await UniTask.Yield();
    }
}