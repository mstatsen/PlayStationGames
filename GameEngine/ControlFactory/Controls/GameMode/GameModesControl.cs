using OxXMLEngine.ControlFactory.Controls;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class GameModesControl : ListItemsControl<ListDAO<GameMode>, GameMode, GameModeEditor, GameField, Game>
    {
        protected override string GetText() => "Play Modes";

        protected override int MaximumItemsCount => 
            TypeHelper.ItemsCount<PlayMode>();
    }
}