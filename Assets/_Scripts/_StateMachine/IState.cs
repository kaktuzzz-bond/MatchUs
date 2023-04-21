public interface IState
{
    void Enter(IStateContext context);


    void Exit(IStateContext context);

}