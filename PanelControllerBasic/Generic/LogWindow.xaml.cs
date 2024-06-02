﻿using PanelController.Controller;
using PanelController.PanelObjects;
using PanelController.PanelObjects.Properties;
using System.Windows;

namespace PanelControllerBasic.Generic
{
    public partial class LogWindow : Window, IPanelObject
    {
        private string _format = "/T [/L][/F] /M\n";

        [UserProperty]
        public string Format
        {
            get => _format;
            set
            {
                _format = value;
                if (!_format.EndsWith('\n'))
                    _format += '\n';
                FormatTextBox.Text = _format.Substring(0, _format.Length - 1);
            }
        }

        public LogWindow()
        {
            InitializeComponent();
            Loaded += LogWindow_Loaded;
            Show();
        }

        public LogWindow(string format)
        {
            InitializeComponent();
            Loaded += LogWindow_Loaded;
            Format = format;
            Show();
        }

        private void LogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Logger.HistoricalLog log in Logger.Logs)
                AddLog(log);
            Logger.Logged += (sender, log) => Dispatcher.Invoke(() =>  AddLog(log));
            FormatTextBox.Text = _format.Substring(0, _format.Length - 1);
        }

        private void AddLog(Logger.HistoricalLog log)
        {
            LogBox.Text += log.ToString(Format);
            if (ScrollToEnd.IsChecked ?? false)
            {
                LogBox.CaretIndex = LogBox.Text.Length;
                LogBox.ScrollToEnd();
            }
        }

        private void ApplyFormatButton_Click(object sender, RoutedEventArgs e) => Format = FormatTextBox.Text;
    }
}