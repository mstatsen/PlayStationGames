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
                AccessoryType.RemoteControl => "Remote control",
                AccessoryType.MemoryCard => "Memory Card",
                AccessoryType.Camera => "Camera",
                AccessoryType.Earphones => "Earphones",
                AccessoryType.Charger => "Original charger",
                AccessoryType.Connector => "Connector",
                AccessoryType.Bag => "Bag",
                AccessoryType.Box => "Original box",
                AccessoryType.Cover => "Cover",
                AccessoryType.Other => "Other",
                _ => "",
            };

        public override AccessoryType EmptyValue() =>
            AccessoryType.Joystick;

        public override AccessoryType DefaultValue() =>
            AccessoryType.Joystick;

        public bool SupportByGeneration(ConsoleGeneration generation, AccessoryType type)
        {
            if (type == AccessoryType.MemoryCard)
                return generation is ConsoleGeneration.PS1 or ConsoleGeneration.PS2;

            if (type == AccessoryType.Charger || type == AccessoryType.Cover)
                return generation is ConsoleGeneration.PSP or ConsoleGeneration.PSVita;

            if (type == AccessoryType.RemoteControl)
                return generation is not ConsoleGeneration.PSP and not ConsoleGeneration.PSVita;

            
            return generation switch
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
}