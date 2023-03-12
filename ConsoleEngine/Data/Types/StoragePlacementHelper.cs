using OxXMLEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data.Types
{
    public class StoragePlacementHelper
        : AbstractTypeHelper<StoragePlacement>
    {
        public override string GetName(StoragePlacement value) => 
            value switch
            {
                StoragePlacement.Internal => "Internal",
                StoragePlacement.External => "External",
                _ => "",
            };

        public override StoragePlacement EmptyValue() =>
            StoragePlacement.Internal;

        public override StoragePlacement DefaultValue() =>
            StoragePlacement.Internal;
    }
}