using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class TrophysetTypeHelper : FieldAccordingHelper<GameField, TrophysetType>
    {
        public override TrophysetType EmptyValue() => 
            TrophysetType.NoSet;

        public override TrophysetType DefaultValue() =>
            TrophysetType.Offline;

        public override string GetName(TrophysetType value) =>
            value switch
            {
                TrophysetType.Offline => "Offline only",
                TrophysetType.OfflineOnline => "Contains online",
                TrophysetType.Online => "Online only",
                TrophysetType.NoSet => "No trophyset",
                _ => string.Empty,
            };

        public override string GetShortName(TrophysetType value) =>
            value switch
            {
                TrophysetType.Offline => "Ordinary",
                TrophysetType.OfflineOnline => "Contains online",
                TrophysetType.Online => "Online only",
                TrophysetType.NoSet => "No trophyset",
                _ => string.Empty,
            };

        public override string GetFullName(TrophysetType value) =>
            value switch
            {
                TrophysetType.Offline => "The game trophyset contains only offline trophies",
                TrophysetType.OfflineOnline => "The game trophyset contains offline and online trophies",
                TrophysetType.Online => "The game trophyset contains only online trophies",
                TrophysetType.NoSet => "The game does not support trophies",
                _ => string.Empty,
            };

        public override bool UseToolTipForControl => true;
    }
}