using PanelController.Controller;
using PanelController.PanelObjects;
using PanelController.PanelObjects.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace PanelControllerBasic.General
{
    [ItemName("Log Message")]
    public class LogMessage : IPanelAction
    {
        public delegate string FormatReplaceValueFunction();

        [ItemName]
        public string Name
        {
            get
            {
                if (Message.Length > 5)
                    return $"Log:\"{Message[..4]}\"...";
                return $"Log:\"{Message}\"";
            }
        }

        [UserProperty]
        public Dictionary<string, FormatReplaceValueFunction> Formatters { get; } = new()
        {
            { "/T", () => DateTime.Now.ToString() }
        };

        [UserProperty]
        public string Message { get; set; } = string.Empty;

        [UserProperty]
        public string Sender { get; set; } = string.Empty;

        [UserProperty]
        public Logger.Levels Level { get; set; } = Logger.Levels.Info;

        public LogMessage()
        { }

        public LogMessage(string message, string sender = "", Logger.Levels level = Logger.Levels.Info)
        {
            Message = message;
            Sender = sender;
            Level = level;
        }

        public object? Run()
        {
            string log = Message;
            foreach (var formatter in Formatters)
                log.Replace(formatter.Key, formatter.Value());
            Logger.Log(log, Level, string.IsNullOrEmpty(Sender) ? this : Sender);
            return null;
        }
    }
}
