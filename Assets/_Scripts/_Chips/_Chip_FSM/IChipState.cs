using Cysharp.Threading.Tasks;

public interface IChipState
{
    UniTask Enter(Chip chip);
}