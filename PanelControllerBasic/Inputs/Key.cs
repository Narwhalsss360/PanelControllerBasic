using PanelController.PanelObjects;
using PanelController.PanelObjects.Properties;
using System.Security.AccessControl;
using WindowsInput;
using WindowsInput.Native;

namespace PanelControllerBasic.Inputs
{
    public class Key : IPanelAction
    {
        public enum KeySimulationTypes
        {
            Down,
            Up,
            Press,
            Toggle
        }

        [ItemName]
        public string Name
        {
            get => $"Key-{KeyCode}";
        }

        [UserProperty]
        public VirtualKeyCode KeyCode { get; set; } = VirtualKeyCode.OEM_1;

        [UserProperty]
        public KeySimulationTypes SimulationType { get; set; } = KeySimulationTypes.Down;

        private delegate IKeyboardSimulator SimulateKey(VirtualKeyCode code);

        private readonly Dictionary<KeySimulationTypes, SimulateKey> s_simulators = new()
        {
            { KeySimulationTypes.Down, Contractor.InputSimulator.Keyboard.KeyDown },
            { KeySimulationTypes.Up, Contractor.InputSimulator.Keyboard.KeyUp },
            { KeySimulationTypes.Press, Contractor.InputSimulator.Keyboard.KeyPress },
            { KeySimulationTypes.Toggle, Contractor.KeyToggle }
        };

        public Key()
        {
            Contractor.Init();
        }

        public Key(KeySimulationTypes simulationType, VirtualKeyCode keyCode)
            : this()
        {
            SimulationType = simulationType;
            KeyCode = keyCode;
        }

        public object? Run()
        {
            s_simulators[SimulationType](KeyCode);
            return null;
        }
    }
}
