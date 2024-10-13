using OxDAOEngine.ControlFactory.Controls;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class AppliesToListControl : ListItemsControl<Platforms, Platform, AppliesToEditor, GameField, Game> 
    {
        protected override string GetText() => "Applies To List";
        protected override string ItemName() => "Applies To";
        public AppliesToListControl() : base() { }
    }
}