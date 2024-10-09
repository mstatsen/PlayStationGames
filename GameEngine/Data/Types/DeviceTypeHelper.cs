using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class DeviceTypeHelper : FieldAccordingHelper<GameField, DeviceType>
    {
        public override DeviceType EmptyValue() =>
            DeviceType.Dualshock;

        public override string GetName(DeviceType value) =>
            value switch
            {
                DeviceType.Dualshock => "Dualshock",
                DeviceType.Camera => "Camera",
                DeviceType.MoveController => "Move Controller",
                DeviceType.NavigationController => "Navigation Controller",
                DeviceType.VR => "VR",
                DeviceType.VRCards => "VR Cards",
                DeviceType.Wonderbook => "Wonderbook",
                _ => string.Empty,
            };

        public override string GetShortName(DeviceType value) =>
            value switch
            {
                DeviceType.Dualshock => "DS",
                DeviceType.Camera => "Camera",
                DeviceType.MoveController => "Move",
                DeviceType.NavigationController => "Navigation",
                DeviceType.VR => "VR",
                DeviceType.VRCards => "VRCards",
                DeviceType.Wonderbook => "Wonderbook",
                _ => string.Empty,
            };
    }
}