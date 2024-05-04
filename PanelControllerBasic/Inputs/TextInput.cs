using PanelController.PanelObjects;
using PanelController.PanelObjects.Properties;

namespace PanelControllerBasic.Inputs
{
    public class TextInput : IPanelAction
    {
        [UserProperty]
        public string Text { get; set; } = string.Empty;

        public object? Run()
        {
            Contractor.InputSimulator.Keyboard.TextEntry(Text);
            return null;
        }
    }
}
