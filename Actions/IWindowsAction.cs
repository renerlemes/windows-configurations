public interface IWindowsAction
{
    string Name { get; }

    string Description { get; }

    bool Get();
    void Execute();
    void Undo();
}
