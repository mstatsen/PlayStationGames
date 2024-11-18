using OxDAOEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class ShortAccessoryInfo
    {
        public readonly string Name;
        public readonly AccessoryType Type;
        public readonly JoystickType JoystickType;

        public ShortAccessoryInfo(string name, AccessoryType type, JoystickType joystickType)
        {
            Name = name;
            Type = type;
            JoystickType = joystickType;
        }

        public override string ToString() =>
            Type switch
            {
                AccessoryType.Joystick =>
                    JoystickType is JoystickType.Other
                        ? "Other joystick"
                        : TypeHelper.Name(JoystickType),
                AccessoryType.Other =>
                    Name,
                _ =>
                    TypeHelper.Name(Type)
            };

        public override bool Equals(object? obj) => 
            base.Equals(obj)
            || (obj is ShortAccessoryInfo otherShort
                && Type.Equals(otherShort.Type)
                && JoystickType.Equals(otherShort.JoystickType)
                && Name.Equals(otherShort.Name));

        public override int GetHashCode() => 
            Type.GetHashCode() ^ JoystickType.GetHashCode() ^ Name.GetHashCode();
    }
}
