using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TMLPatcher_UI.Views
{
    public class FileListing : UserControl
    {
        public FileListing()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}