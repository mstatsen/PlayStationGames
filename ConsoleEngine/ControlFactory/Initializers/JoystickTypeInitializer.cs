using OxLibrary.Controls;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Initializers
{
    public class JoystickTypeInitializer : TypedComboBoxInitializer<JoystickType>
    {
        public PSConsole Console { get; set; }
        public JoystickTypeInitializer(PSConsole console) : base() => 
            Console = console;

        public override void InitControl(Control control) => 
            ((OxComboBox)control).SelectedItem =
                Console is null
                    ? joystickTypeHelper.DefaultValue()
                    : TypeHelper.Helper<ConsoleGenerationHelper>().DefaultJoystick(Console.Generation);

        private readonly JoystickTypeHelper joystickTypeHelper = TypeHelper.Helper<JoystickTypeHelper>();

        public override bool AvailableValue(JoystickType value) =>
            Console is null 
            || joystickTypeHelper
                .SupportByGeneration(Console.Generation, Console.Model, value);
    }
}