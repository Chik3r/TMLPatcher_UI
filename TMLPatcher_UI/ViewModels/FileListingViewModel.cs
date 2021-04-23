using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMLPatcher_UI.Models;

namespace TMLPatcher_UI.ViewModels
{
    public class FileListingViewModel : ViewModelBase
    {
        public FileListingViewModel(IEnumerable<FileItem> files) => Files = new ObservableCollection<FileItem>(files);

        public ObservableCollection<FileItem> Files { get; }
    }
}