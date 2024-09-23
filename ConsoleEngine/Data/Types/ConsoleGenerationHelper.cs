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

        public override ConsoleGeneration EmptyValue() =>
            ConsoleGeneration.Unknown;

        public override ConsoleGeneration DefaultValue() =>
            ConsoleGeneration.PS4;

        public bool FolderSupport(ConsoleGeneration generation) 
        {
            if (!StorageSupport(generation))
                return false;

            return generation switch
            {
                ConsoleGeneration.PS5 or 
                ConsoleGeneration.PS4 or 
                ConsoleGeneration.PS3 or 
                ConsoleGeneration.PSVita => 
                    true,
                _ => 
                    false,
            };
        }

        public bool StorageSupport(ConsoleGeneration generation) => 
            generation != ConsoleGeneration.PS1;

        public int MaxAccountsCount(ConsoleGeneration generation, FirmwareType firmwareType) =>
            generation switch
            {
                ConsoleGeneration.PS1 or
                ConsoleGeneration.PS2 =>
                    0,
                ConsoleGeneration.PSP when firmwareType == FirmwareType.Custom =>
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
                ConsoleGeneration.PS5 => ConsoleIcons.ps5,
                ConsoleGeneration.PS4 => ConsoleIcons.ps4,
                ConsoleGeneration.PS3 => ConsoleIcons.ps3,
                ConsoleGeneration.PSVita => ConsoleIcons.psvita,
                ConsoleGeneration.PS2 => ConsoleIcons.ps2,
                ConsoleGeneration.PSP => ConsoleIcons.psp,
                ConsoleGeneration.PS1 => ConsoleIcons.ps1,
                _ => OxIcons.Close
            };
    }
}