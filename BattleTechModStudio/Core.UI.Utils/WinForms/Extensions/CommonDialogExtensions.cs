using System.Windows;
using System.Windows.Forms;

namespace Core.UI.Utils.WinForms.Extensions
{
    public static class CommonDialogExtensions
    {
        public static DialogResult ShowDialog(this CommonDialog dialog, Window parent)
        {
            return dialog.ShowDialog(new Wpf32Window(parent));
        }
    }
}