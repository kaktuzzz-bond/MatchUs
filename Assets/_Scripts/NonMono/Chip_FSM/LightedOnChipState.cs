using Cysharp.Threading.Tasks;
using Game;

namespace NonMono.Chip_FSM
{
    public class LightedOnChipState : IChipState
    {
        public  void Enter(Chip chip)
        {
            chip.Activate(true);

            LightOnAsync(chip).Forget();
        }
    
        private async UniTaskVoid LightOnAsync(Chip chip)
        {
            await chip.Fade(1f);
        }
    }
}