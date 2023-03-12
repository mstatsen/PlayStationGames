using OxLibrary.Controls;
using OxXMLEngine.ControlFactory.Initializers;
using OxXMLEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Initializers
{
    public class PlayModeInitializer : TypedComboBoxInitializer<PlayMode>
    {
        public ListDAO<GameMode>? ExistingModes;

        public override void InitControl(Control control)
        {
            OxComboBox ComboBox = (OxComboBox)control;

            if (ComboBox.Items.Count > 0)
                ComboBox.SelectedIndex = 0;
        }

        public override bool AvailableValue(PlayMode value) =>
            ExistingModes == null
            || !ExistingModes.Contains((l) => l.PlayMode == value);
    }
}