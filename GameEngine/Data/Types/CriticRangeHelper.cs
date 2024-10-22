using OxDAOEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class CriticRangeHelper : AbstractTypeHelper<CriticRange>
    {
        public override CriticRange EmptyValue() =>
            default!;

        public override string GetName(CriticRange value) =>
            value switch
            {
                CriticRange.Bad => "Bad",
                CriticRange.Medium => "Medium",
                CriticRange.Good => "Good",
                CriticRange.Best => "Best",
                _ => "Unknown",
            };

        public CriticRange Range(int score) => 
            score switch
            {
                < 0 => CriticRange.Unknown,
                < 30 => CriticRange.Bad,
                < 60 => CriticRange.Medium,
                < 90 => CriticRange.Good,
                _ => CriticRange.Best
            };
    }
}