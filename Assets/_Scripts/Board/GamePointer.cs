using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game;
using NonMono;
using UnityEngine;

namespace Board
{
    public class GamePointer : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private const float FadeDuration = 0.05f;

        private ObjectPool<GamePointer> _parentPool;

        public async UniTask Show(ObjectPool<GamePointer> parentPool)
        {
            _parentPool = parentPool;
            
            gameObject.SetActive(true);

            await spriteRenderer
                    .DOFade(1f, FadeDuration)
                    .ToUniTask();
        }


        public async UniTask HideAsync()
        {
            await spriteRenderer
                    .DOFade(0f, GameManager.Instance.gameData.chipFadeTime)
                    .ToUniTask();

           _parentPool.Release(this);
        }


        public GamePointer SetPosition(Vector3 position)
        {
            transform.position = position;

            return this;
        }


        public GamePointer SetName(string newName)
        {
            transform.name = newName;

            return this;
        }
    }
}