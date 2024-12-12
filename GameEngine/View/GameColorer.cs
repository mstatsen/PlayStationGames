using OxLibrary;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.View;

public class GameColorer : ItemColorer<GameField, Game>
{
    public override Color BaseColor(Game? item) =>
        new OxColorHelper(TypeHelper.BackColor(item?.SourceType)).Darker(7);
}