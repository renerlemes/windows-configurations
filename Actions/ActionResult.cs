namespace Windows.Configurations
{
    /// <summary>
    /// Distingue os motivos de uma ação não ser aplicada: recusar o UAC é decisão do
    /// usuário e não precisa de aviso, já um bloqueio por política precisa ser explicado.
    /// </summary>
    internal enum ActionResult
    {
        Applied,
        Cancelled,
        Denied,
        Failed
    }
}
