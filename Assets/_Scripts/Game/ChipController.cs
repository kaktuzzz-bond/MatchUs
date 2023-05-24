using System;
using Board;
using Cysharp.Threading.Tasks;
using NonMono;
using NonMono.Commands;
using UI;
using UnityEngine;

namespace Game
{
    public class ChipController : Singleton<ChipController>
    {
        private PointerPool _pointerPool;


        private void Awake()
        {
            _pointerPool = new PointerPool(
                    GameManager.Instance.gameData.selectorPrefab.GetComponent<GamePointer>(),
                    GameManager.Instance.gameData.hintPrefab.GetComponent<GamePointer>());
        }


        public async UniTask AddChips()
        {
            var allChipInfos = ChipInfo.ExtractInfos(ChipRegistry.ActiveChips);

            var infos = ChipInfo.GetClonedInfo(allChipInfos, ChipRegistry.Counter);

            await CommandLogger.AddCommand(new AddChipsCommand(infos));
        }


        public async UniTask ShuffleChips()
        {
            await CommandLogger.AddCommand(new ShuffleCommand());
        }


        public void ShowHints()
        {
            var inGameChips = ChipRegistry.ActiveChips;

            int count = inGameChips.Count;

            for (int i = 0; i < count - 1; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    Chip first = inGameChips[i];
                    Chip second = inGameChips[j];

                    if (!ChipComparer.CompareChips(first, second)) continue;

                    Debug.Log("SHOW HINTS");

                    _pointerPool.ShowPointer(PointerPool.Hint, first.transform.position).Forget();

                    _pointerPool.ShowPointer(PointerPool.Hint, second.transform.position).Forget();

                    return;
                }
            }

            //Debug.Log("======== NO HINTS ========");

            GameGUI.Instance.ShowInfo();
        }


        public async UniTask ProcessTappedChip(Chip chip)
        {
            var tapped = ChipComparer.HandleTap(chip);

            if (tapped == null)
            {
                _pointerPool.HideAllVisible().Forget();

                return;
            }

            if (tapped.Length == 1)
            {
                _pointerPool.HideAllVisible().Forget();

                await _pointerPool.ShowPointer(PointerPool.Selector, tapped[0].transform.position);

                return;
            }

            await _pointerPool.ShowPointer(PointerPool.Selector, tapped[0].transform.position);

            await _pointerPool.HideAllVisible();

            Matching(tapped[0], tapped[1]).Forget();
        }


        private async UniTask Matching(Chip first, Chip second)
        {
            await CommandLogger.AddCommand(new FadeOutCommand(first, second));

            Debug.Log("Draw line here");
        }


        public void Restart()
        {
            CommandLogger.Reset();

            ChipRegistry.Reset();

            var infos = ChipInfo.GetStartChipInfoArray();

            GameManager.Instance.gameData.SetStartArrayInfos(infos);
        }
    }
}