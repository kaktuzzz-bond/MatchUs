using Board;
using Game;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NonMono
{
    public class PointerPool
    {
        private GamePointer _selector;

        private GamePointer[] _hints;

        private GameManager _gameManager;

        private int _selectorCounter;


        public void Init()
        {
            _gameManager = GameManager.Instance;

            Transform selector = Object.Instantiate(
                    _gameManager.gameData.selectorPrefab,
                    _gameManager.gameData.pointerParent);

            selector.gameObject.SetActive(false);

            _selector = selector.GetComponent<GamePointer>();

            _selector.SetName(_selector.tag);

            _hints = new GamePointer[2];

            for (int i = 0; i < 2; i++)
            {
                Transform hint = Object.Instantiate(
                        _gameManager.gameData.hintPrefab,
                        _gameManager.gameData.pointerParent);

                hint.gameObject.SetActive(false);

                _hints[i] = hint.GetComponent<GamePointer>();

                _hints[i].SetName(_hints[i].tag);
            }
        }


        public void ShowSelector(Vector3 position)
        {
            _selector.SetPosition(position).Show();
        
        }


        public void HideSelector()
        {
            _selector.HideAsync().Forget();
        }


        public void ShowHints(Vector3 firstPosition, Vector3 secondPosition)
        {
            _hints[0].SetPosition(firstPosition).Show();

            _hints[1].SetPosition(secondPosition).Show();
        }


        public void HideHints()
        {
            _hints[0].HideAsync().Forget();

            _hints[1].HideAsync().Forget();
        }
    }
}