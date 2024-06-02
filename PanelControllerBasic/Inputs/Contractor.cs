using PanelController.Controller;
using WindowsInput;
using WindowsInput.Native;

namespace PanelControllerBasic.Inputs
{
    internal static class Contractor
    {
        public static readonly InputSimulator InputSimulator = new();

        private static List<VirtualKeyCode> s_toggledKeys = new();

        private static bool s_initialized = false;

        public static void Init()
        {
            if (s_initialized)
                return;
            s_initialized = true;
            Main.Deinitialized += (sender, args) =>
            {
                foreach (var code in s_toggledKeys)
                    InputSimulator.Keyboard.KeyUp(code);
            };
        }

        public static IKeyboardSimulator KeyToggle(VirtualKeyCode code)
        {
            if (s_toggledKeys.Contains(code))
            {
                s_toggledKeys.Remove(code);
                return InputSimulator.Keyboard.KeyUp(code);
            }
            else
            {
                s_toggledKeys.Add(code);
                return InputSimulator.Keyboard.KeyDown(code);
            }
        }
    }
}
