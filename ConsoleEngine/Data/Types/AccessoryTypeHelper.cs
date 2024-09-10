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
                AccessoryType.Other => "Other",
                AccessoryType.Connector => "Connector",
                _ => "",
            };

        public override AccessoryType EmptyValue() =>
            AccessoryType.Joystick;

        public override AccessoryType DefaultValue() =>
            AccessoryType.Joystick;

        public bool SupportByGeneration(ConsoleGeneration generation, AccessoryType type) => 
            generation switch
            {
                ConsoleGeneration.PSP => 
                    type != AccessoryType.Joystick,
                ConsoleGeneration.PSVita => 
                    type != AccessoryType.Joystick && 
                    type != AccessoryType.Camera,
                _ => true,
            };

    }
}