using OxDAOEngine.ControlFactory.Controls;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public class StoragesControl : ListItemsControl<Storages, Storage, StorageEditor, ConsoleField, PSConsole>
    {
        protected override string GetText() => "Storages";
    }
}