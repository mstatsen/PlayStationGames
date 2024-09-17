using OxDAOEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data.Types
{
    public class FirmwareTypeHelper
        : AbstractStyledTypeHelper<FirmwareType>
    {
        public override string GetName(FirmwareType value) => 
            value switch
            {
                FirmwareType.Official => "Official",
                FirmwareType.Custom => "Custom",
                _ => string.Empty,
            };

        public override FirmwareType EmptyValue() =>
            FirmwareType.Official;

        public override Color GetBaseColor(FirmwareType value) => 
            value switch
            {
                FirmwareType.Official => Color.FromArgb(245, 251, 232),
                FirmwareType.Custom => Color.FromArgb(255, 250, 210),
                _ => Color.FromArgb(246, 246, 246),
            };

        public override Color GetFontColor(FirmwareType value) => default;
    }
}
