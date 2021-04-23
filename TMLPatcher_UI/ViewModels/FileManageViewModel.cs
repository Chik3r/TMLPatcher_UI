using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using TMLPatcher_UI.Models;

namespace TMLPatcher_UI.ViewModels
{
    public class FileManageViewModel : ViewModelBase
    {
        private bool _currentlyExtracting;
        private int _extractProgress;

        public FileManageViewModel(IEnumerable<FileItem> files)
        {
            Files = new ObservableCollection<FileItem>(files);
            CurrentlyExtracting = false;
        }

        public ObservableCollection<FileItem> Files { get; }

        public bool CurrentlyExtracting
        {
            get => _currentlyExtracting;
            set => this.RaiseAndSetIfChanged(ref _currentlyExtracting, value);
        }

        public int ExtractProgress
        {
            get => _extractProgress;
            set => this.RaiseAndSetIfChanged(ref _extractProgress, value);
        }

        public void ExtractMod()
        {
            CurrentlyExtracting = !CurrentlyExtracting;
            var rand = new System.Random();
            ExtractProgress = rand.Next(0, 101);
        }
    }
}