using Cysharp.Threading.Tasks;

public interface ICommand
{
    UniTask Execute();


    UniTask Undo();
}