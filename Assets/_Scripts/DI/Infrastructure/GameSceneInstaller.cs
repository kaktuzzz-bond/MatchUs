using UnityEngine;
using Zenject;

namespace DI.Infrastructure
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private CameraInputHandler cameraInputHandler;

        [SerializeField]
        private CameraDirector cameraDirector;

        [SerializeField]
        private InputService inputService;


        public override void InstallBindings()
        {
            BindInputService();

            BindCamera();
        }


        private void BindCamera()
        {
            Container.Bind<Camera>()
                     .FromInstance(mainCamera)
                     .AsSingle()
                     .NonLazy();

            Container.Bind<CameraInputHandler>()
                     .FromInstance(cameraInputHandler)
                     .AsSingle()
                     .NonLazy();

            Container.Bind<CameraDirector>()
                     .FromInstance(cameraDirector)
                     .AsSingle()
                     .NonLazy();
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