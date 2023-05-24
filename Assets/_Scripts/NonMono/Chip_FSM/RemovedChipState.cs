using Cysharp.Threading.Tasks;
using Game;

namespace NonMono.Chip_FSM
{
    public class RemovedChipState : IChipState
    {
        public void Enter(Chip chip)
        {
            LightOutAsync(chip).Forget();
        }


        private async UniTaskVoid LightOutAsync(Chip chip)
        {
            await UniTask.WhenAll(
                    chip.RemoveFromBoardAsync(),
                    chip.Fade(0f));

            ChipRegistry.UnregisterFromGame(chip);

            chip.Activate(false);
        }
    }
}