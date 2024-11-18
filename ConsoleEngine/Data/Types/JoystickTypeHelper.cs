using OxDAOEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data.Types
{
    public class JoystickTypeHelper
        : AbstractTypeHelper<JoystickType>
    {
        public override string GetName(JoystickType value) =>
            value switch
            {
                JoystickType.PlayStation => "PlayStation controller",
                JoystickType.DualAnalog => "DualAnalog",
                JoystickType.Dualshock => "Dualshock",
                JoystickType.Dualshock2 => "Dualshock 2",
                JoystickType.Sixaxis => "Sixaxis",
                JoystickType.Dualshock3 => "Dualshock 3",
                JoystickType.Dualshock4 => "Dualshock 4",
                JoystickType.Dualsense5 => "Dualshock 5",
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
                JoystickType.PlayStation or
                JoystickType.DualAnalog or
                JoystickType.Dualshock or
                JoystickType.Dualshock2 or
                JoystickType.Sixaxis or
                JoystickType.Dualshock3 or
                JoystickType.Dualshock4 or
                JoystickType.Dualsense5 or
                JoystickType.DSReplica or
                JoystickType.DS2Replica or
                JoystickType.DS3Replica or
                JoystickType.DS4Replica or
                JoystickType.DS5Replica or
                JoystickType.Other => true,
                _ => false
            };

        public bool WithSticks(JoystickType type) =>
            type is not JoystickType.PlayStation
                and not JoystickType.MoveController;

        public bool SupportByGeneration(ConsoleGeneration generation, ConsoleModel model, JoystickType type)
        {
            if (generation is ConsoleGeneration.PSP || 
                generation is ConsoleGeneration.PSVita)
                return false;

            if (type is JoystickType.Other)
                return true;

            return generation switch
            {
                ConsoleGeneration.PS5 =>
                    type is JoystickType.Dualsense5 or
                    JoystickType.Dualshock4 or
                    JoystickType.DS5Replica or
                    JoystickType.MoveController or
                    JoystickType.NavigationController,
                ConsoleGeneration.PS4 =>
                    type is JoystickType.Dualshock4 or
                    JoystickType.DS4Replica or
                    JoystickType.MoveController or
                    JoystickType.NavigationController,
                ConsoleGeneration.PS3 =>
                    type is JoystickType.Dualshock4 or
                    JoystickType.Dualshock3 or
                    JoystickType.DS3Replica or
                    JoystickType.Dualsense5 or
                    JoystickType.Dualshock4 or
                    JoystickType.Sixaxis or
                    JoystickType.MoveController or
                    JoystickType.NavigationController,
                ConsoleGeneration.PS2 =>
                    type is JoystickType.Dualshock2 or
                    JoystickType.DS2Replica or
                    JoystickType.Dualshock,
                ConsoleGeneration.PS1 =>
                    type is JoystickType.Dualshock2 or
                    JoystickType.Dualshock or
                    JoystickType.DSReplica,
                ConsoleGeneration.PSVita when model is ConsoleModel.PSVitaTV  => 
                    type is JoystickType.Dualshock3 or
                    JoystickType.Dualshock4,
                ConsoleGeneration.PSP when model is ConsoleModel.PSPGO =>
                    type is JoystickType.Dualshock3,
                _ => false,
            };
        }

        public bool IsOficial(JoystickType joystickType) =>
            joystickType switch
            {
                JoystickType.PlayStation or
                JoystickType.DualAnalog or
                JoystickType.Dualshock or
                JoystickType.Dualshock2 or
                JoystickType.Sixaxis or
                JoystickType.Dualshock3 or
                JoystickType.Dualshock4 or
                JoystickType.Dualsense5 or
                JoystickType.MoveController or
                JoystickType.NavigationController =>
                    true,
                _ => false
            };
    }
}