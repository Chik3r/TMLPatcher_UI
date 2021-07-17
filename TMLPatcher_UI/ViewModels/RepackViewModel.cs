using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using ReactiveUI;
using TML.Files.ModLoader.Files;
using TML.Patcher.Packing;
using TMLPatcher_UI.Models;

namespace TMLPatcher_UI.ViewModels
{
    public class RepackViewModel : ViewModelBase
    {
        private FileItem _selectedFile;
        private string _modName;
        private string _modVersion;
        private string _tmlVersion;
        private bool _canRepack;

        public RepackViewModel()
        {
            Files = new ObservableCollection<FileItem>();
        }

        public ObservableCollection<FileItem> Files { get; }

        public FileItem SelectedFile
        {
            get => _selectedFile;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedFile, value);
                ModName = value.FileName;
            }
        }

        public string ModName
        {
            get => _modName;
            set
            {
                this.RaiseAndSetIfChanged(ref _modName, value);
                UpdateCanRepack();
            }
        }

        public string ModVersion
        {
            get => _modVersion;
            set
            {
                this.RaiseAndSetIfChanged(ref _modVersion, value);
                UpdateCanRepack();
            }
        }

        public string TmlVersion
        {
            get => _tmlVersion;
            set
            {
                this.RaiseAndSetIfChanged(ref _tmlVersion, value);
                UpdateCanRepack();
            }
        }

        public bool CanRepack
        {
            get => _canRepack;
            set => this.RaiseAndSetIfChanged(ref _canRepack, value);
        }

        public void UpdateCanRepack() =>
            CanRepack = !(string.IsNullOrWhiteSpace(ModName) || !Version.TryParse(ModVersion, out _) ||
                          !Version.TryParse(TmlVersion, out _));

        public void Populate()
        {
            Files.Clear();

            DirectoryInfo extractDir = Directory.CreateDirectory(Program.ExtractDirectory);

            // Get all directories in extract dir
            foreach (DirectoryInfo dir in extractDir.EnumerateDirectories())
                Files.Add(new FileItem(dir.Name, dir.FullName));
        }

        public void RepackMod()
        {
            DirectoryInfo repackDir = new(SelectedFile.FilePath);
            string targetFilePath = Path.ChangeExtension(Path.Combine(Program.RepackDirectory, SelectedFile.FileName), ".tmod");

            Directory.CreateDirectory(Program.RepackDirectory);

            ModData data = new(ModName, Version.Parse(ModVersion), Version.Parse(TmlVersion));

            // TODO: convert threads to option
            RepackRequest request = new(repackDir, targetFilePath, data, 4);

            request.ExecuteRequest();
        }
    }
}