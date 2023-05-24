using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Board;
using Cysharp.Threading.Tasks;
using Game;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NonMono
{
    public class PointerPool
    {
        public static string Selector { get; private set; }

        public static string Hint { get; private set; }

        private readonly Dictionary<string, ObjectPool<GamePointer>> _pools;

        public readonly List<GamePointer> _pointers = new();


        public PointerPool(GamePointer selector, GamePointer hint)
        {
            Selector = selector.tag;

            Hint = hint.tag;

            _pools = new Dictionary<string, ObjectPool<GamePointer>>()
            {
                    { Selector, new ObjectPool<GamePointer>(selector) },
                    { Hint, new ObjectPool<GamePointer>(hint) }
            };
        }


        public async UniTask ShowPointer(string name, Vector3 position)
        {
            if (!_pools.ContainsKey(name)) return;

            GamePointer pointer = _pools[name]
                    .Get()
                    .SetName(name)
                    .SetPosition(position);

            _pointers.Add(pointer);

            await pointer.Show(_pools[name]);
        }


        // public void HidePointer(GamePointer pointer)
        // {
        //     _pointers.Remove(pointer);
        //
        //     pointer.HideAsync().Forget();
        // }


        public async UniTask HideAllVisible()
        {
            foreach (GamePointer pointer in _pointers)
            {
                pointer.HideAsync().Forget();
            }

            _pointers.Clear();

            await UniTask.Yield();
        }
    }
}