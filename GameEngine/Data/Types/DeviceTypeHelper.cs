using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class DeviceTypeHelper : FieldAccordingHelper<GameField, DeviceType>
    {
        public override DeviceType EmptyValue() =>
            default!;

        public override string GetName(DeviceType value) =>
            value switch
            {
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
                DeviceType.Camera => "Camera",
                DeviceType.MoveController => "Move",
                DeviceType.NavigationController => "Navigation",
                DeviceType.VR => "VR",
                DeviceType.VRCards => "VRCards",
                DeviceType.Wonderbook => "Wonderbook",
                _ => string.Empty,
            };

        public List<DeviceType> Available(PlatformType platform)
        {
            List<DeviceType> result = new();

            switch (platform)
            { 
                case PlatformType.PS3:
                    result.Add(DeviceType.Camera);
                    result.Add(DeviceType.MoveController);
                    result.Add(DeviceType.NavigationController);
                    result.Add(DeviceType.Wonderbook);
                    break;
                case PlatformType.PS4:
                case PlatformType.PS5:
                    result.Add(DeviceType.Camera);
                    result.Add(DeviceType.MoveController);
                    result.Add(DeviceType.VR);
                    break;
                case PlatformType.PSP:
                    result.Add(DeviceType.Camera);
                    break;
                case PlatformType.PSVita:
                    result.Add(DeviceType.VRCards);
                    break;
            }

            return result;
        }
    }
}