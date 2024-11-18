using OxDAOEngine.ControlFactory.Initializers;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Types;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.ControlFactory.Controls.Initializers
{
    public class PlaystationPlatformTypeInitializer : TypedComboBoxInitializer<PlatformType>
    {
        public readonly ListDAO<Platform> ExistingTypes = new();
        public Game? Game { get; set; }

        public override bool AvailableValue(PlatformType value) =>
            base.AvailableValue(value)
            && TypeHelper.Helper<PlatformTypeHelper>().IsPSNPlatform(value)
            && (Game is null 
                || Game.PlatformType.Equals(value)
                || Game.ReleasePlatforms.Contains(p => p.Type.Equals(value)))
            && (ExistingTypes.Count is 0 
                || !ExistingTypes.Contains(p => p.Type.Equals(value)));

        public PlaystationPlatformTypeInitializer() { }
    }
}