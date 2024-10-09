using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class SeriesListControl : ListItemsControl<ListDAO<Series>, Series, SeriesEditor, GameField, Game> 
    {
        protected override string GetText() => "Serieses";
        protected override string ItemName() => "Series";
    }
}
