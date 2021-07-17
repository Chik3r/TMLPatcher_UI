using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TMLPatcher_UI.ViewModels;

namespace TMLPatcher_UI.Views
{
    public class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        
        public MainWindow()
        {
            Instance = this;
            
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ChangeSelectedTab(object? sender, SelectionChangedEventArgs e)
        {
            if (e.Source is not TabControl tabControl) return;

            if (tabControl.SelectedItem is not TabItem tabItem) return;

            switch (tabItem.Content)
            {
                case RepackViewModel repackViewModel:
                    repackViewModel.Populate();
                    break;
                case FileManageViewModel extractViewModel:
                    extractViewModel.Populate();
                    break;
            }
        }
    }
}