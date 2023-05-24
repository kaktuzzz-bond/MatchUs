using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Board
{
    public class GamePointer : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private const float FadeDuration = 0.05f;


        public void Show()
        {
            gameObject.SetActive(true);

            spriteRenderer
                    .DOFade(1f, FadeDuration);
        }


        public async UniTaskVoid HideAsync()
        {
            await spriteRenderer
                    .DOFade(0f, FadeDuration)
                    .ToUniTask();

            gameObject.SetActive(false);
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