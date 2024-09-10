using OxLibrary.Controls;
using OxXMLEngine.ControlFactory.Initializers;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Initializers
{
    public class AccessoryTypeInitializer : TypedComboBoxInitializer<AccessoryType>
    {
        public PSConsole Console { get; set; }
        public AccessoryTypeInitializer(PSConsole console) : base() => 
            Console = console;

        public override void InitControl(Control control)
        {
            OxComboBox ComboBox = (OxComboBox)control;

            if (ComboBox.Items.Count > 0)
                ComboBox.SelectedIndex = 0;
        }

        public override bool AvailableValue(AccessoryType value) =>
            Console == null 
            || TypeHelper.Helper<AccessoryTypeHelper>().SupportByGeneration(Console.Generation, value);
    }
}