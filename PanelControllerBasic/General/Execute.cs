﻿using PanelController.PanelObjects;
using PanelController.PanelObjects.Properties;
using System.Diagnostics;
using System.IO;

namespace PanelControllerBasic.General
{
    public class Execute : IPanelAction
    {
        [UserProperty]
        public string Path { get; set; } = string.Empty;

        [UserProperty]
        public bool IsRunning { get => _process is not null && !_process.HasExited; }

        [UserProperty]
        public List<string> Arguments { get; set; } = new();

        [ItemName]
        public string Name
        {
            get => $"Execute:{ProcessFile.Name}";
        }

        private FileInfo ProcessFile { get => new(Path); }

        private Process? _process = null;

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
