using PanelController.PanelObjects;
using PanelController.PanelObjects.Properties;
using System.Diagnostics;

namespace PanelControllerBasic.General
{
    public class Execute : IPanelAction
    {
        [UserProperty]
        public string Path { get; set; } = string.Empty;

        private FileInfo ProcessFile { get => new FileInfo(Path); }

        private Process? _process = null;

        [UserProperty]
        public bool IsRunning { get => _process is null ? false : !_process.HasExited; }

        public Execute() { }

        public Execute(string path)
        {
            Path = path;
        }

        public object? Run()
        {
            if (!ProcessFile.Exists)
                return $"No file found with path {Path}.";
            if (_process is not null)
                return "Process is already running.";
            if (ProcessFile.Extension.ToLower() != ".exe")
                return "File type not supported";
            _process = Process.Start(Path);
            _process.Exited += (sender, args) => { _process = null; };
            return null;
        }
    }
}
