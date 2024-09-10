using OxXMLEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data.Types
{
    public class AccessoryTypeHelper
        : AbstractTypeHelper<AccessoryType>
    {
        public override string GetName(AccessoryType value) => 
            value switch
            {
                AccessoryType.Joystick => "Joystick",
                AccessoryType.Camera => "Camera",
                AccessoryType.Earphones => "Earphones",
                _ => "",
            };

        public override AccessoryType EmptyValue() =>
            AccessoryType.Joystick;

        public override AccessoryType DefaultValue() =>
            AccessoryType.Joystick;
    }
}