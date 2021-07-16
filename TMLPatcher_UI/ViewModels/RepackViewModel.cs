using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using TML.Patcher.Packing;
using TMLPatcher_UI.Models;
using TMLPatcher_UI.Views;

namespace TMLPatcher_UI.ViewModels
{
    public class RepackViewModel : ViewModelBase
    {
        private bool _currentlyExtracting;
        private int _extractProgress;
        private FileItem _selectedFile;
        private ProgressReporter _progressReporter;

        public RepackViewModel()
        {
            Files = new ObservableCollection<FileItem>();
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

        public FileItem SelectedFile
        {
            get => _selectedFile;
            set => this.RaiseAndSetIfChanged(ref _selectedFile, value);
        }

        public async void Populate()
        {
            // Create a new folder dialog
            OpenFolderDialog folderDialog = new();
            folderDialog.Title = "Select your Mods folder";

            // Wait for the user to select the folder
            string folder = await folderDialog.ShowAsync(MainWindow.Instance);
            
            // Check if the folder exists
            if (string.IsNullOrEmpty(folder))
                return;

            if (!Directory.Exists(folder))
                return;

            // Get all files in the folder and add them to the listbox
            string[] files = Directory.GetFiles(folder, "*.tmod");
            foreach (string file in files) Files.Add(new FileItem(Path.GetFileNameWithoutExtension(file), file));
        }

        public void ExtractMod()
        {
            // Start the unpack request with a new progress reporter
            // The progress reporter will update the progress bar
            _progressReporter = new ProgressReporter();
            UnpackRequest request = new UnpackRequest(
                Directory.CreateDirectory(Path.Combine(Program.ExtractDirectory, SelectedFile.FileName)),
                SelectedFile.FilePath, 4, _progressReporter);

            _progressReporter.UpdateProgress += UpdateProgress;

            // Start a new task so that the unpack request runs in a separate thread from the ui
            Task.Run(() => request.ExecuteRequest());
            _progressReporter.Start();

            // Disable the extract mod button
            CurrentlyExtracting = true;
        }

        private void UpdateProgress(int i)
        {
            // Set the extract progress to the progress
            ExtractProgress = i;

            // Check if the extraction has completed
            if (i == 100)
            {
                _progressReporter.Finish();
                CurrentlyExtracting = false;

                // Format the path to the extracted folder
                string folderPath = Path.Combine(Program.ExtractDirectory, SelectedFile.FileName);
                if (!folderPath.EndsWith(Path.DirectorySeparatorChar))
                    folderPath += Path.DirectorySeparatorChar;
                
                // Open the folder
                Process.Start(new ProcessStartInfo
                {
                    FileName = folderPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }
    }
}