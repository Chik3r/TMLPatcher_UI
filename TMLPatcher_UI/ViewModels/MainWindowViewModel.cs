using System.Collections.Generic;
using TMLPatcher_UI.Models;

namespace TMLPatcher_UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            // TODO: Store path to mods folder in configuration file
            
            FileManage = new FileManageViewModel(new FileItem[] {});
            FileManage.Populate();

            RepackModel = new RepackViewModel();
            RepackModel.Populate();
        }
        
        public FileManageViewModel FileManage { get; }
        public RepackViewModel RepackModel { get; }
    }
}