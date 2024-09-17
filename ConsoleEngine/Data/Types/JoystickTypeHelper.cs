using OxDAOEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data.Types
{
    public class JoystickTypeHelper
        : AbstractTypeHelper<JoystickType>
    {
        public override string GetName(JoystickType value) =>
            value switch
            {
                JoystickType.Dualshock => "Dualshock",
                JoystickType.Dualshock2 => "Dualshock 2",
                JoystickType.Dualshock3 => "Dualshock 3",
                JoystickType.Dualshock4 => "Dualshock 4",
                JoystickType.Dualshock5 => "Dualshock 5",
                JoystickType.DSReplica => "Replica of Dualshock",
                JoystickType.DS2Replica => "Replica of Dualshock 2",
                JoystickType.DS3Replica => "Replica of Dualshock 3",
                JoystickType.DS4Replica => "Replica of Dualshock 4",
                JoystickType.DS5Replica => "Replica of Dualshock 5",
                JoystickType.MoveController => "Move controller",
                JoystickType.NavigationController => "Navigation Controller",
                JoystickType.Other => "Other",
                _ => string.Empty,
            };

        public override JoystickType EmptyValue() =>
            JoystickType.Other;

        public override JoystickType DefaultValue() =>
            JoystickType.Other;

        public bool IsColored(JoystickType type) =>
            type switch
            {
                JoystickType.Dualshock or
                JoystickType.Dualshock2 or
                JoystickType.Dualshock3 or
                JoystickType.Dualshock4 or
                JoystickType.Dualshock5 or
                JoystickType.DSReplica or
                JoystickType.DS2Replica or
                JoystickType.DS3Replica or
                JoystickType.DS4Replica or
                JoystickType.DS5Replica => true,
                _ => false
            };

        public bool SupportByGeneration(ConsoleGeneration generation, JoystickType type)
        {
            if (generation == ConsoleGeneration.PSP || generation == ConsoleGeneration.PSVita)
                return false;

            if (type == JoystickType.Other)
                return true;

            return generation switch
            {
                ConsoleGeneration.PS5 => 
                    type == JoystickType.Dualshock5 ||
                    type == JoystickType.DS5Replica ||
                    type == JoystickType.MoveController ||
                    type == JoystickType.NavigationController,
                ConsoleGeneration.PS4 => 
                    type == JoystickType.Dualshock4 ||
                    type == JoystickType.DS4Replica ||
                    type == JoystickType.MoveController ||
                    type == JoystickType.NavigationController,
                ConsoleGeneration.PS3 => 
                    type == JoystickType.Dualshock3 ||
                    type == JoystickType.DS3Replica ||
                    type == JoystickType.MoveController ||
                    type == JoystickType.NavigationController,
                ConsoleGeneration.PS2 => 
                    type == JoystickType.Dualshock2 ||
                    type == JoystickType.DS2Replica,
                ConsoleGeneration.PS1 => 
                    type == JoystickType.Dualshock ||
                    type == JoystickType.DSReplica,
                _ => false,
            };
        }
    }
}