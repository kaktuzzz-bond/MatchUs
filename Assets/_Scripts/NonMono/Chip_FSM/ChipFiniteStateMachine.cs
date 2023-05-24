using Game;

namespace NonMono.Chip_FSM
{
    public class ChipFiniteStateMachine
    {
        private IChipState _currentState;

        private readonly LightedOnChipState _lightOn = new();

        private readonly LightedOffChipState _lightOff = new();

        private readonly RemovedChipState _removed = new();

        private readonly Chip _chip;

        public ChipFiniteStateMachine(Chip chip)
        {
            _chip = chip;
        
            _currentState = new RemovedChipState();
        
            //SetState(chip.CurrentState);
        }


        public void LightOn()
        {
            SetState(_lightOn);
        }


        public void LightOff()
        {
            SetState(_lightOff);
        }


        public void Removed()
        {
            SetState(_removed);
        }
        private void SetState(IChipState newState)
        {
            _currentState = newState;

            _currentState.Enter(_chip);
        }
    }
}