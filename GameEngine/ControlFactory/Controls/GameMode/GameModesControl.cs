using OxDAOEngine.ControlFactory.Controls;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class GameModesControl : ListItemsControl<ListDAO<GameMode>, GameMode, GameModeEditor, GameField, Game>
    {
        protected override string GetText() => "Play Modes";
        protected override string ItemName() => "Play Mode";

        protected override int MaximumItemsCount => 
            TypeHelper.ItemsCount<PlayMode>();
    }
}