using OxDAOEngine.ControlFactory.Controls;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public class FoldersControl : ListItemsControl<Folders, Folder, FolderEditor, ConsoleField, PSConsole> 
    {
        protected override string GetText() => "Folders";
        protected override string ItemName() => "Folder";
    }
}