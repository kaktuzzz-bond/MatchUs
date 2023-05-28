using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DI.Infrastructure
{
    public class CameraDirector : MonoBehaviour
    {
        private Camera _camera;

        private Vector3 _topBoundPoint;

        private Vector3 _bottomBoundPoint;

        private float _camToNextPositionDistance;


        [Inject]
        private void Construct(Camera mainCamera)
        {
            _camera = mainCamera;
        }


        public async UniTask SetPosition(Vector3 position)
        {
            _camera.transform.position = position;

            await UniTask.Yield();
        }


        public async UniTask SetOrthographicSize(float orthographicSize)
        {
            _camera.orthographicSize = orthographicSize;

            await UniTask.WaitForEndOfFrame(this);
        }
    }
}