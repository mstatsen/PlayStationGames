using OxDAOEngine.Data.Types;

namespace PlayStationGames.GameEngine.Summary
{
    public class LevelValueTypeGroupHelper
        : AbstractTypeHelper<LevelValueTypeGroup>
    {
        public override LevelValueTypeGroup EmptyValue() =>
            LevelValueTypeGroup.Points;

        public override string GetName(LevelValueTypeGroup value) => 
            value switch
            {
                LevelValueTypeGroup.Points => "Points",
                LevelValueTypeGroup.Level => "Level",
                LevelValueTypeGroup.Progress => "Progress",
                LevelValueTypeGroup.Trophies => "Trophies",
                _ => "Other",
            };
    }
}
