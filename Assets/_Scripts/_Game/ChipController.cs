using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class ChipController : Singleton<ChipController>
{
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
        Debug.Log("SHOW HINTS");
    }


    public void ProcessTappedChip(Chip chip)
    {
        var tapped = ChipComparer.HandleTap(chip);

        if (tapped == null)
        {
            Board.Instance.HideSelector();

            return;
        }

        if (tapped.Length == 1)
        {
            Board.Instance.ShowSelector(chip.transform.position);

            return;
        }

        Matching(tapped[0], tapped[1]).Forget();
    }


    private async UniTask Matching(Chip first, Chip second)
    {
        await CommandLogger.AddCommand(new FadeOutCommand(first, second));
    }
}