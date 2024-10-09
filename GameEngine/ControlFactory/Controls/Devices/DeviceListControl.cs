using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class DeviceListControl : ListItemsControl<ListDAO<Device>, Device, DeviceEditor, GameField, Game> 
    {
        protected override string GetText() => "Devices";
        protected override string ItemName() => "Device";
    }
}
