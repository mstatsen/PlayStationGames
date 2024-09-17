using OxDAOEngine.Data.Types;

namespace PlayStationGames.GameEngine.Summary
{
    public class LevelValueTypeHelper : AbstractTypeHelper<LevelValueType>
    {
        public override string GetName(LevelValueType valueType) => 
            valueType switch
            {
                LevelValueType.Points => "Points",
                LevelValueType.Level => "Level",
                LevelValueType.LevelOld => "Level (old)",
                LevelValueType.CountPlatinum => "Platinum",
                LevelValueType.CountGold => "Gold",
                LevelValueType.CountSilver => "Silver",
                LevelValueType.CountBronze => "Bronze",
                LevelValueType.CountFromDLC => "From DLC",
                LevelValueType.CountNet => "Net",
                LevelValueType.Progress => "Progress",
                _ => string.Empty,
            };

        public LevelValueTypeGroup Group(LevelValueType type) => 
            type switch
            {
                LevelValueType.Level or 
                LevelValueType.LevelOld => 
                    LevelValueTypeGroup.Level,
                LevelValueType.CountPlatinum or 
                LevelValueType.CountGold or 
                LevelValueType.CountSilver or 
                LevelValueType.CountBronze or 
                LevelValueType.CountFromDLC or 
                LevelValueType.CountNet => 
                    LevelValueTypeGroup.Trophies,
                _ => 
                    LevelValueTypeGroup.Points,
            };

        public override LevelValueType EmptyValue() =>
            LevelValueType.Points;
    }
}