using System.Windows;
using System.Windows.Controls;

namespace Core.UI.Utils.Utils
{
    public class MarginSetter
    {
        // Using a DependencyProperty as the backing store for Margin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached(
            "Margin",
            typeof(Thickness),
            typeof(MarginSetter),
            new UIPropertyMetadata(new Thickness(), MarginSetter.MarginChangedCallback));

        public static Thickness GetMargin(DependencyObject obj)
        {
            return (Thickness) obj.GetValue(MarginSetter.MarginProperty);
        }

        public static void MarginChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Make sure this is put on a panel
            var panel = sender as Panel;
            if (panel == null)
            {
                return;
            }

            panel.Loaded += MarginSetter.PanelLoaded;
        }

        public static void SetMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(MarginSetter.MarginProperty, value);
        }

        private static void PanelLoaded(object sender, RoutedEventArgs e)
        {
            var panel = sender as Panel;

            // Go over the children and set margin for them:
            foreach (var child in panel.Children)
            {
                var fe = child as FrameworkElement;
                if (fe == null)
                {
                    continue;
                }

                fe.Margin = MarginSetter.GetMargin(panel);
            }
        }
    }
}