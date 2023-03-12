using OxXMLEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class TrophysetAccessibilityHelper : AbstractTypeHelper<TrophysetAccessibility>
    {
        public override TrophysetAccessibility EmptyValue() => TrophysetAccessibility.Ordinary;

        public override string GetName(TrophysetAccessibility value) => 
            value switch
            {
                TrophysetAccessibility.Ordinary => "Ordinary",
                TrophysetAccessibility.NeverGet => "Complete impossible",
                TrophysetAccessibility.NoSet => "No trophyset",
                _ => string.Empty,
            };
    }
}