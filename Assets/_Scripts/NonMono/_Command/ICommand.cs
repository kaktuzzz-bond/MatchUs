using Cysharp.Threading.Tasks;

public interface ICommand
{
    void Execute();


    UniTask Undo();
}