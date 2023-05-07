#define ENABLE_LOGS
using DG.Tweening;

public class ActiveGameState : IGameState
{
    public void Enter(GameFiniteStateMachine context)
    {
        Logger.Debug("Active Game State entered ");

        DOTween.SetTweensCapacity(5000, 1000);

        GameSceneGUI.Instance.Init();
    }


    public void Exit(GameFiniteStateMachine context)
    {
        throw new System.NotImplementedException();
    }
}