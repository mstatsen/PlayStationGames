using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class TrophysetAccessHelper : FieldAccordingHelper<GameField, TrophysetAccess>
    {
        public override TrophysetAccess EmptyValue() => 
            TrophysetAccess.Ordinary;

        public override string GetName(TrophysetAccess value) => 
            value switch
            {
                TrophysetAccess.Ordinary => "Ordinary",
                TrophysetAccess.NeverGet => "Complete impossible",
                TrophysetAccess.NoSet => "No trophyset",
                _ => string.Empty,
            };
    }
}