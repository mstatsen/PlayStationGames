using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.PSNEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class GameLinkTypeHelper : PSNLinkTypeHelper<GameField>
    {
        protected override GameField GetExtractFieldName() => GameField.Links;
    }
}
