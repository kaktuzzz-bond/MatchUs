using Cysharp.Threading.Tasks;

namespace NonMono.Commands
{
    public interface ICommand
    {
        UniTask Execute();


        UniTask Undo();
    }
}