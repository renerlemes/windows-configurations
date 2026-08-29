using System.Windows.Forms;

namespace Windows.Configurations
{
    internal static class ActionBinding
    {
        public static void Load(CheckBox checkBox, IWindowsAction action)
        {
            checkBox.Checked = action.Get();
        }

        public static void Bind(CheckBox checkBox, IWindowsAction action)
        {
            checkBox.CheckedChanged += (_, _) =>
            {
                if (checkBox.Checked)
                    action.Execute();
                else
                    action.Undo();
            };
        }
    }
}
