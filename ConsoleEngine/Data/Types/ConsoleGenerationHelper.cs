using OxLibrary;
using OxDAOEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data.Types
{
    public class ConsoleGenerationHelper
        : AbstractTypeHelper<ConsoleGeneration>
    {
        public override string GetName(ConsoleGeneration value) => 
            value switch
            {
                ConsoleGeneration.PS5 => "PS5",
                ConsoleGeneration.PS4 => "PS4",
                ConsoleGeneration.PS3 => "PS3",
                ConsoleGeneration.PSVita => "PSVita",
                ConsoleGeneration.PS2 => "PS2",
                ConsoleGeneration.PSP => "PSP",
                ConsoleGeneration.PS1 => "PS1",
                _ => "Unknown",
            };

        public override string GetFullName(ConsoleGeneration value) =>
            value switch
            {
                ConsoleGeneration.PS5 => "PlayStation 5",
                ConsoleGeneration.PS4 => "PlayStation 4",
                ConsoleGeneration.PS3 => "PlayStation 3",
                ConsoleGeneration.PSVita => "PlayStation Vita",
                ConsoleGeneration.PS2 => "PlayStation 2",
                ConsoleGeneration.PSP => "PlayStation Portable",
                ConsoleGeneration.PS1 => "PlayStation 1",
                _ => "Unknown",
            };

        public override ConsoleGeneration EmptyValue() =>
            ConsoleGeneration.PS4;


        public bool FolderSupport(ConsoleGeneration generation) =>
            generation is 
                ConsoleGeneration.PS5
                or ConsoleGeneration.PS4
                or ConsoleGeneration.PS3
                or ConsoleGeneration.PSVita;

        public bool StorageSupport(ConsoleGeneration generation) => 
            generation is not ConsoleGeneration.PS1;

        public int MaxAccountsCount(ConsoleGeneration generation, FirmwareType firmwareType) =>
            generation switch
            {
                ConsoleGeneration.PS1 or
                ConsoleGeneration.PS2 =>
                    0,
                ConsoleGeneration.PSP when firmwareType is FirmwareType.Custom =>
                    0,
                ConsoleGeneration.PS3 or
                ConsoleGeneration.PS4 or
                ConsoleGeneration.PS5 =>
                    16,
                _ => 1
            };

        public Bitmap Icon(ConsoleGeneration generation) =>
            generation switch
            {
                ConsoleGeneration.PS5 => OxIcons.PS5,
                ConsoleGeneration.PS4 => OxIcons.PS4,
                ConsoleGeneration.PS3 => OxIcons.PS3,
                ConsoleGeneration.PSVita => OxIcons.PSVita,
                ConsoleGeneration.PS2 => OxIcons.PS2,
                ConsoleGeneration.PSP => OxIcons.PSP,
                ConsoleGeneration.PS1 => OxIcons.PS1,
                _ => OxIcons.Close
            };

        public JoystickType DefaultJoystick(ConsoleGeneration generation) =>
            generation switch
            {
                ConsoleGeneration.PS5 => 
                    JoystickType.Dualsense5,
                ConsoleGeneration.PS4 => 
                    JoystickType.Dualshock4,
                ConsoleGeneration.PSVita or
                ConsoleGeneration.PSP or
                ConsoleGeneration.PS3 => 
                    JoystickType.Dualshock3,
                ConsoleGeneration.PS2 => 
                    JoystickType.Dualshock2,
                ConsoleGeneration.PS1 => 
                    JoystickType.Dualshock,
                _ => 
                    JoystickType.Other,
            };

        public override bool UseFullNameForControl => true;
    }
}