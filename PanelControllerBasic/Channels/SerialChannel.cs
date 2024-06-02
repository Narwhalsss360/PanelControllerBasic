using PanelController.PanelObjects;
using PanelController.PanelObjects.Properties;
using System.IO.Ports;

namespace PanelControllerBasic.Channels
{
    public class SerialChannel : IChannel
    {
        [ItemName]
        public string Name
        {
            get => $"Serial Port {_port.PortName}";
        }

        private readonly SerialPort _port = new() { PortName = "COM1", BaudRate = 115200, DtrEnable = true, RtsEnable = true };

        public bool IsOpen => _port.IsOpen;

        public event EventHandler<byte[]>? BytesReceived;

        public string Status
        {
            get => $"{_port.PortName}@{_port.BaudRate}baud:{(IsOpen ? "Opened" : "Closed")}";
        }

        [UserProperty]
        public int MillisecondsWait { get; set; } = 50;

        public SerialChannel()
        {
            _port.DataReceived += DataReceived;
        }

        public SerialChannel(string portName, int baudrate, int millisecondsWait = 50)
            : this()
        {
            _port.PortName = portName;
            _port.BaudRate = baudrate;
            MillisecondsWait = millisecondsWait;
        }

        private void ProcessReceive()
        {
            byte[] bytes = new byte[_port.BytesToRead];
            _port.Read(bytes, 0, bytes.Length);
            BytesReceived?.Invoke(this, bytes);
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Eof)
                return;
            ProcessReceive();
        }

        public void Close()
        {
            if (_port.IsOpen)
            {
                try { _port.Close(); } catch { _port.Dispose(); } 
            }
        }

        public object? Open()
        {
            if (IsOpen)
                return null;

            try
            {
                _port.Open();
            }
            catch (Exception e)
            {
                return e;
            }

            _port.DiscardInBuffer();
            _port.DiscardOutBuffer();
            Task.Delay(MillisecondsWait).Wait();
            return null;
        }

        public object? Send(byte[] data)
        {
            try
            {
                _port.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                return e;
            }
            return null;
        }

        private static List<string> s_oldPortNames = new();

        [IChannel.Detector]
        public static IChannel[] Detect()
        {
            List<IChannel> channels = new();

            string[] currentPortNames = SerialPort.GetPortNames();
            s_oldPortNames = s_oldPortNames.Where(portName => currentPortNames.Contains(portName)).ToList();

            return Array.ConvertAll<string, IChannel>(
                currentPortNames.Where(port => !s_oldPortNames.Contains(port)).ToArray(), (port) =>
            {
                s_oldPortNames.Add(port);
                return new SerialChannel(port, 115200);
            });
        }
    }
}
