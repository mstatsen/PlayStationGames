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
            this.Console = console;

        public override void InitControl(Control control)
        {
            OxComboBox ComboBox = (OxComboBox)control;

            if (ComboBox.Items.Count > 0)
                ComboBox.SelectedIndex = 0;
        }

        public override bool AvailableValue(JoystickType value) =>
            Console == null 
            || TypeHelper.Helper<JoystickTypeHelper>().SupportByGeneration(Console.Generation, value);
    }
}