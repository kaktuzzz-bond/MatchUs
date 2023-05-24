using UnityEngine;
using UnityEngine.SceneManagement;

namespace NonMono.Game_FSM
{
    public class InitialGameState : IGameState
    {
        public void Enter(GameFiniteStateMachine context)
        {
            Debug.Log("MainScreen: Initial Game State entered ");
        }


        public void Exit(GameFiniteStateMachine context)
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}