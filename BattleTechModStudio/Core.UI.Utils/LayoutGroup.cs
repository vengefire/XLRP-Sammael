using System.Windows.Controls;

namespace Core.UI.Utils
{
    public class LayoutGroup : StackPanel
    {
        public LayoutGroup()
        {
            Grid.SetIsSharedSizeScope(this, true);
        }
    }
}