namespace NonMono.Game_FSM
{
    public interface IGameState
    {
        void Enter(GameFiniteStateMachine context);


        void Exit(GameFiniteStateMachine context);
    }
}