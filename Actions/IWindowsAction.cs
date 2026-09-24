public interface IWindowsAction
{
    string Name { get; }

    string Description { get; }

    /// <summary>
    /// Grava fora do perfil do usuário (HKLM ou plano de energia) e só se aplica em um
    /// processo elevado. A leitura em <see cref="Get"/> continua funcionando sem elevação.
    /// </summary>
    bool RequiresElevation { get; }

    bool Get();
    void Execute();
    void Undo();
}
