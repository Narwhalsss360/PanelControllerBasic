using PanelController.Controller;
using PanelController.PanelObjects;
using PanelController.PanelObjects.Properties;

namespace PanelControllerBasic.General
{
    [ItemName("Log Message")]
    public class LogMessage : IPanelAction
    {
        public delegate string FormatReplaceValueFunction();

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
