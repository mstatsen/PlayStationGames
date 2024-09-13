using OxXMLEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class PlatformFamilyHelper
        : AbstractTypeHelper<PlatformFamily>
    {
        public override PlatformFamily EmptyValue() => 
            PlatformFamily.Other;

        public override string GetName(PlatformFamily value) => 
            GetShortName(value);

        public override string GetShortName(PlatformFamily value) => 
            value switch
            {
                PlatformFamily.Sony => "Sony",
                PlatformFamily.Microsoft => "Microsoft",
                PlatformFamily.Nintendo => "Nintendo",
                PlatformFamily.Mobile => "Mobile",
                _ => "Other",
            };
    }
}