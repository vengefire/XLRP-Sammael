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
            this.InitializeComponent();
            this.Root.DataContext = this;
        }

        public string Label
        {
            get => (string)this.GetValue(LabelledTextBox.LabelProperty);
            set => this.SetValue(LabelledTextBox.LabelProperty, value);
        }

        public string Text
        {
            get => (string)this.GetValue(LabelledTextBox.TextProperty);
            set => this.SetValue(LabelledTextBox.TextProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool)this.GetValue(LabelledTextBox.ReadOnlyProperty);
            set => this.SetValue(LabelledTextBox.ReadOnlyProperty, value);
        }
    }
}