using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

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

        Board.Instance.ReleasePointer(this);
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


    public GamePointer SetParent(Transform parent)
    {
        transform.SetParent(parent);

        return this;
    }
}