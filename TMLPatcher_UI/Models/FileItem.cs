namespace TMLPatcher_UI.Models
{
    public class FileItem
    {
        public FileItem(string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
        }
        
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public override string ToString() => FileName;
    }
}