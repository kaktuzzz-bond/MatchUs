using Cysharp.Threading.Tasks;

public interface IGameState
{
    void Enter(GameFiniteStateMachine context);


    //void Exit(GameFiniteStateMachine context);
}