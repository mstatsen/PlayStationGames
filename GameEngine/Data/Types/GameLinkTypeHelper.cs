using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class GameLinkTypeHelper : FieldAccordingHelper<GameField, GameLinkType>, ILinkHelper<GameField>
    {
        public GameField ExtractFieldName => GameField.Links;

        public override GameLinkType EmptyValue() => GameLinkType.Other;

        public override Color GetBaseColor(GameLinkType value) =>
            value switch
            {
                GameLinkType.PSN =>
                    Color.FromArgb(130, 180, 130),
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
                GameLinkType.PSN =>
                    Color.FromArgb(130, 180, 130),
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
                GameLinkType.PSN => "PSN",
                GameLinkType.Stratege => "Stratege",
                GameLinkType.PSNProfiles => "PSNProfiles",
                _ => "Other"
            };

        public bool IsMandatoryLink(object value) => !GameLinkType.Other.Equals(value);
    }
}