using UnityEngine;
using Zenject;

namespace DI.Infrastructure
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private Camera inputCamera;

        [SerializeField]
        private InputService inputService;


        public override void InstallBindings()
        {
            BindInputService();

            BindInputCamera();
        }


        private void BindInputCamera()
        {
            Container.Bind<Camera>()
                     .FromInstance(inputCamera)
                     .AsSingle();
        }


        private void BindInputService()
        {
            Container.Bind<IInputService>()
                     .FromInstance(inputService)
                     .AsSingle()
                     .NonLazy();
        }
    }
}