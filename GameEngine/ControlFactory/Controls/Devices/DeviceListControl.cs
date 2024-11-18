using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxLibrary;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class DeviceListControl : ListItemsControl<ListDAO<Device>, Device, DeviceEditor, GameField, Game> 
    {
        protected override string GetText() => "Devices";
        protected override string ItemName() => "Device";
        public DeviceListControl() : base() => 
            GetMaximumCount += GetMaximumCountHandler;

        private int GetMaximumCountHandler() => 
            OwnerDAO is null 
                ? -1 
                : TypeHelper.Helper<DeviceTypeHelper>().
                    Available(OwnerDAO.PlatformType).Count;
    }
}