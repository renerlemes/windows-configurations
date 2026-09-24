using System;
using System.Security;
using System.Windows.Forms;

namespace Windows.Configurations
{
    internal static class ActionBinding
    {
        public static void Load(CheckBox checkBox, IWindowsAction action)
        {
            try
            {
                checkBox.Checked = action.Get();
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException or SecurityException)
            {
                // Máquina gerenciada pode bloquear até a leitura da chave.
                checkBox.Checked = false;
            }
        }

        public static void Bind(CheckBox checkBox, IWindowsAction action)
        {
            EventHandler handler = null;

            handler = (_, _) =>
            {
                ActionResult result = Apply(action, checkBox.Checked);

                if (result == ActionResult.Applied)
                    return;

                // Devolve o estado anterior sem reentrar no próprio evento.
                checkBox.CheckedChanged -= handler;
                checkBox.Checked = !checkBox.Checked;
                checkBox.CheckedChanged += handler;

                Explain(result, action);
            };

            checkBox.CheckedChanged += handler;
        }

        private static ActionResult Apply(IWindowsAction action, bool enabled)
        {
            if (action.RequiresElevation && !ElevatedAction.IsElevated)
                return ElevatedAction.Run(action, enabled);

            try
            {
                if (enabled)
                    action.Execute();
                else
                    action.Undo();

                return ActionResult.Applied;
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException or SecurityException)
            {
                return ActionResult.Denied;
            }
            catch (Exception)
            {
                return ActionResult.Failed;
            }
        }

        private static void Explain(ActionResult result, IWindowsAction action)
        {
            // Recusar o prompt do UAC é decisão do usuário: não precisa de aviso.
            if (result == ActionResult.Cancelled)
                return;

            string message = result == ActionResult.Denied
                ? $"Não foi possível aplicar \"{action.Name}\".\n\nA política de segurança deste computador bloqueou a alteração. "
                  + "Em máquinas gerenciadas pela empresa, essa configuração costuma ser controlada pelo administrador de TI."
                : $"Não foi possível aplicar \"{action.Name}\".";

            MessageBox.Show(
                message,
                "Windows Configurations",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}
