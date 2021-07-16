using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TMLPatcher_UI.Views
{
    public class RepackView : UserControl
    {
        public RepackView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}