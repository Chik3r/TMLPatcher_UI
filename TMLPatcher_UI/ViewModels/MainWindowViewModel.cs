using System.Collections.Generic;
using TMLPatcher_UI.Models;

namespace TMLPatcher_UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            // TODO: Populate the file listing automatically by getting all files inside the mods folder
            // TODO: Pop-up that asks the user for the path to the mods folder
            // TODO: Store path to mods folder in configuration file
            List<FileItem> files = new()
            {
                new FileItem("CalamityMod", "C:\\Users\\ikerv\\Documents\\My Games\\Terraria\\ModLoader\\Mods\\CalamityMod.tmod"),
                new FileItem("CheatSheet", "C:\\Users\\ikerv\\Documents\\My Games\\Terraria\\ModLoader\\Mods\\CheatSheet.tmod")
            };

            // TODO: Remove debug items
            for (int i = 0; i < 50; i++)
                files.Add(new FileItem($"Item {i}", "C:\\Users\\ikerv\\Documents\\My Games\\Terraria\\ModLoader\\Mods\\CheatSheet.tmod"));

            FileManage = new FileManageViewModel(files);
        }
        
        public FileManageViewModel FileManage { get; }
    }
}