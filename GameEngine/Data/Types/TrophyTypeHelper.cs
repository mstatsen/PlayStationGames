using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class TrophyTypeHelper : AbstractTypeHelper<TrophyType>
    {
        public override TrophyType EmptyValue() => TrophyType.Platinum;

        public override string GetName(TrophyType value) => 
            value switch
            {
                TrophyType.Platinum => "Platinum",
                TrophyType.Gold => "Gold",
                TrophyType.Silver => "Silver",
                TrophyType.Bronze => "Bronze",
                TrophyType.FromDLC => "DLC",
                TrophyType.Net => "Net",
                _ => string.Empty,
            };

        public GameField EarnedGameField(TrophyType type) => 
            type switch
            {
                TrophyType.Platinum => GameField.EarnedPlatinum,
                TrophyType.Gold => GameField.EarnedGold,
                TrophyType.Silver => GameField.EarnedSilver,
                TrophyType.Bronze => GameField.EarnedBronze,
                TrophyType.FromDLC => GameField.EarnedFromDLC,
                TrophyType.Net => GameField.EarnedNet,
                _ => GameField.Field,
            };
    }
}