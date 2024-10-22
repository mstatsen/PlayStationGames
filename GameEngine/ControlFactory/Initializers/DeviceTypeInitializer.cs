using OxDAOEngine.ControlFactory.Initializers;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Types;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Data;

namespace PlayStationGames.GameEngine.ControlFactory.Controls.Initializers
{
    public class DeviceTypeInitializer : TypedComboBoxInitializer<DeviceType>
    {
        public readonly ListDAO<Device> ExistingTypes = new();
        public Game? Game { get; set; }

        private readonly DeviceTypeHelper deviceTypeHelper = TypeHelper.Helper<DeviceTypeHelper>();

        public override bool AvailableValue(DeviceType value) => 
            base.AvailableValue(value)
            && (Game == null || deviceTypeHelper.Available(Game.PlatformType).Contains(value))
            && (ExistingTypes.Count == 0 || !ExistingTypes.Contains(d => d.Type == value));

        public DeviceTypeInitializer() { }
    }
}