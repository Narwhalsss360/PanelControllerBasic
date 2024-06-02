using PanelController.PanelObjects;
using PanelController.PanelObjects.Properties;

namespace PanelControllerBasic.Inputs
{
    public class TextInput : IPanelAction
    {
        [UserProperty]
        public string Text { get; set; } = string.Empty;

        [ItemName]
        public string Name
        {
            get
            {
                if (Text.Length > 5)
                    return $"TextInput:\"{Text[..4]}\"...";
                return $"TextInput:\"{Text}\"";
            }
        }

        public object? Run()
        {
            Contractor.InputSimulator.Keyboard.TextEntry(Text);
            return null;
        }
    }
}
