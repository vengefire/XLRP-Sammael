using System.Windows;
using System.Windows.Controls;

namespace UI.Core.Utils.Controls
{
    /// <summary>
    ///     Interaction logic for LabelledTextBox.xaml
    /// </summary>
    public partial class LabelledTextBox : UserControl
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            "Label",
            typeof(string),
            typeof(LabelledTextBox),
            new FrameworkPropertyMetadata("Unnamed Label"));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(LabelledTextBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly",
            typeof(bool),
            typeof(LabelledTextBox),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public LabelledTextBox()
        {
            InitializeComponent();
            Root.DataContext = this;
        }

        public string Label
        {
            get => (string) GetValue(LabelledTextBox.LabelProperty);
            set => SetValue(LabelledTextBox.LabelProperty, value);
        }

        public string Text
        {
            get => (string) GetValue(LabelledTextBox.TextProperty);
            set => SetValue(LabelledTextBox.TextProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool) GetValue(LabelledTextBox.ReadOnlyProperty);
            set => SetValue(LabelledTextBox.ReadOnlyProperty, value);
        }
    }
}