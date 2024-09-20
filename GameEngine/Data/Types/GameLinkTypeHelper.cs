using OxDAOEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class GameLinkTypeHelper : AbstractStyledTypeHelper<GameLinkType>
    {
        public override GameLinkType EmptyValue() => GameLinkType.Walkthrough;

        public override Color GetBaseColor(GameLinkType value) =>
            value switch
            {
                GameLinkType.Stratege =>
                    Color.FromArgb(180, 170, 130),
                GameLinkType.PSNProfiles =>
                    Color.FromArgb(130, 160, 180),
                _ =>
                    Color.FromArgb(170, 150, 200),
            };

        public override Color GetFontColor(GameLinkType value) =>
            value switch
            {
                GameLinkType.Stratege =>
                    Color.FromArgb(180, 170, 130),
                GameLinkType.PSNProfiles =>
                    Color.FromArgb(130, 160, 180),
                _ =>
                    Color.FromArgb(170, 150, 200),
            };

        public override string GetName(GameLinkType value) =>
            value switch
            {
                GameLinkType.Stratege => "Stratege",
                GameLinkType.PSNProfiles => "PSNProfiles",
                _ => "Walkthrough"
            };
    }
}