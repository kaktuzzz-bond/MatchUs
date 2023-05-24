using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Board
{
    [RequireComponent(typeof(TrailRenderer))]
    public class Line : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;


        private void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
        }


        public async UniTask DrawLineAsync(Vector3[] positions, Color startColor, Color endColor, float showTime)
        {
            _trailRenderer.startColor = startColor;

            _trailRenderer.endColor = endColor;

            foreach (Vector3 pos in positions)
            {
                transform.position = pos;

                await UniTask.Yield();
            }
        
            _trailRenderer.time = showTime;
        }
    }
}