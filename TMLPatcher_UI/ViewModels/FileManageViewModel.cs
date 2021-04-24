using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using TML.Patcher.Backend.Packing;
using TMLPatcher_UI.Models;
using TMLPatcher_UI.Views;

namespace TMLPatcher_UI.ViewModels
{
    public class FileManageViewModel : ViewModelBase
    {
        private bool _currentlyExtracting;
        private int _extractProgress;
        private FileItem _selectedFile;
        private ProgressReporter _progressReporter;

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
        
        public FileItem SelectedFile
        {
            get => _selectedFile;
            set => this.RaiseAndSetIfChanged(ref _selectedFile, value);
        }

        public async void Populate()
        {
            OpenFolderDialog folderDialog = new();
            folderDialog.Title = "Select your Mods folder";
            
            var folder = await folderDialog.ShowAsync(MainWindow.Instance);
            if (string.IsNullOrEmpty(folder))
                return;
            
            System.Console.WriteLine($"Folder: {folder}");
            if (!Directory.Exists(folder))
                return;

            string[] files = Directory.GetFiles(folder, "*.tmod");
            foreach (string file in files) Files.Add(new FileItem(Path.GetFileNameWithoutExtension(file), file));
        }

        public void ExtractMod()
        {
            // CurrentlyExtracting = !CurrentlyExtracting;
            // var rand = new System.Random();
            // ExtractProgress = rand.Next(0, 101);
            
            _progressReporter = new ProgressReporter();
            UnpackRequest request = new UnpackRequest(
                Directory.CreateDirectory(Path.Combine(Program.ExtractDirectory, SelectedFile.FileName)),
                SelectedFile.FilePath, 4, _progressReporter);

            _progressReporter.UpdateProgress += i =>
            {
                ExtractProgress = i;
                if (i == 100)
                {
                    _progressReporter.Finish();
                    CurrentlyExtracting = false;
                }
            };
            
            Task.Run(() => request.ExecuteRequest());
            _progressReporter.Start();
            
            CurrentlyExtracting = true;
        }
    }
}