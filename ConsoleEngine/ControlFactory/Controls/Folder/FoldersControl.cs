using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxXMLEngine.ControlFactory.Controls;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public class FoldersControl : ListItemsControl<Folders, Folder, FolderEditor, ConsoleField, PSConsole> 
    {
        protected override string GetText() => "Folders";
    }
}