using OxXMLEngine.ControlFactory.Controls;
using OxXMLEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class DLCListControl : ListItemsControl<ListDAO<DLC>, DLC, DLCEditor, GameField, Game> 
    {
        protected override string GetText() => 
            "DLC List";
    }
}
