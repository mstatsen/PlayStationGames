using OxXMLEngine.ControlFactory.Controls;
using OxXMLEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class TagListControl : ListItemsControl<ListDAO<Tag>, Tag, TagEditor, GameField, Game> 
    {
        protected override string GetText() => 
            "Tag List";
    }
}
