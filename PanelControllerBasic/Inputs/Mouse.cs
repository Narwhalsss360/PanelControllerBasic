using PanelController.PanelObjects;
using PanelController.PanelObjects.Properties;

namespace PanelControllerBasic.Inputs
{
    public class MouseButton : IPanelAction
    {
        [UserProperty]
        public int ButtonID { get; private set; }

        public object? Run()
        {
            Contractor.InputSimulator.Mouse.XButtonDoubleClick(ButtonID);
            return null;
        }
    }
}
