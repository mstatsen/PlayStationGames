using OxDAOEngine.Data.Types;

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
                AccessoryType.StickCover => "Reserve stick cover",
                AccessoryType.Other => "Other",
                AccessoryType.Documents => "Documents",
                AccessoryType.Wonderbook => "Wonderbook",
                AccessoryType.VR => "VR System",
                AccessoryType.ARCards => "AR Cards",
                _ => string.Empty,
            };

        public override string GetShortName(AccessoryType value) =>
            value switch
            {
                AccessoryType.Joystick => "Joystick",
                AccessoryType.RemoteControl => "Remote",
                AccessoryType.MemoryCard => "MemoryCard",
                AccessoryType.Camera => "Camera",
                AccessoryType.Earphones => "Earphones",
                AccessoryType.Charger => "Charger",
                AccessoryType.Connector => "Connector",
                AccessoryType.Bag => "Bag",
                AccessoryType.Box => "Box",
                AccessoryType.Cover => "Cover",
                AccessoryType.StickCover => "StickCover",
                AccessoryType.Other => "Other",
                AccessoryType.Documents => "Documents",
                AccessoryType.Wonderbook => "Wonderbook",
                AccessoryType.VR => "VR",
                AccessoryType.ARCards => "ARCards",
                _ => string.Empty,
            };

        public override AccessoryType EmptyValue() =>
            AccessoryType.Joystick;

        public override AccessoryType DefaultValue() =>
            AccessoryType.Joystick;

        public bool Named(AccessoryType type, JoystickType joystickType) =>
            type switch
            {
                AccessoryType.Joystick => 
                    joystickType == JoystickType.Other,
                AccessoryType.Camera or
                AccessoryType.Earphones or
                AccessoryType.Connector or
                AccessoryType.Other => 
                    true,
                _ => 
                    false
            };

        public bool SupportModelCode(AccessoryType type, JoystickType joystickType) =>
            type == AccessoryType.Joystick
                ? TypeHelper.Helper<JoystickTypeHelper>().IsOficial(joystickType)
                : type is 
                    AccessoryType.VR or
                    AccessoryType.Camera or
                    AccessoryType.Earphones;

        public bool SupportByGeneration(ConsoleGeneration generation, AccessoryType type) => 
            type switch
            {
                AccessoryType.MemoryCard => 
                    generation is ConsoleGeneration.PS1 or 
                        ConsoleGeneration.PS2,
                AccessoryType.Charger or 
                AccessoryType.Cover => 
                    generation is ConsoleGeneration.PSP or 
                    ConsoleGeneration.PSVita,
                AccessoryType.VR =>
                    generation is ConsoleGeneration.PS4 or
                        ConsoleGeneration.PS5,
                AccessoryType.Wonderbook =>
                    generation is ConsoleGeneration.PS3,
                AccessoryType.ARCards =>
                    generation is ConsoleGeneration.PSVita,
                AccessoryType.RemoteControl => 
                    generation is not ConsoleGeneration.PSP and 
                    not ConsoleGeneration.PSVita,
                _ => generation switch
                    {
                        ConsoleGeneration.PSP =>
                            type != AccessoryType.Joystick,
                        ConsoleGeneration.PSVita =>
                            type != AccessoryType.Joystick &&
                            type != AccessoryType.Camera,
                        _ => true,
                    },
            };
    }
}