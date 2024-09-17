using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.ControlFactory.Controls;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public class AccessoriesControl : ListItemsControl<Accessories, Accessory, AccessoryEditor, ConsoleField, PSConsole>
    {
        protected override string GetText() => "Accessories";
    }
}