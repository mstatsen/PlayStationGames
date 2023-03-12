using OxXMLEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data.Types
{
    public class SuitableConsoleGames : List<SuitableConsoleGame>
    {
        public void Add(PlatformType platformType, Source? source, bool? licensed)
        {
            List<bool> licensedList = new();
            List<Source> sourceList = new();

            if (licensed == null || licensed == true)
                licensedList.Add(true);

            if (licensed == null || licensed == false)
                licensedList.Add(false);

            if (source == null)
            { 
                foreach (Source s in TypeHelper.All<Source>())
                    if (TypeHelper.Helper<SourceHelper>().InstallationsSupport(s))
                        sourceList.Add(s);
            }
            else
            {
                Source inSource = (Source)source!;
                if (TypeHelper.Helper<SourceHelper>().InstallationsSupport(inSource))
                    sourceList.Add(inSource);
            }

            foreach (bool l in licensedList)
                foreach (Source s in sourceList)
                    Add(new SuitableConsoleGame(platformType, s, l));
        }

        public void Add(PlatformType platformType, bool? licensed) => 
            Add(platformType, null, licensed);

        public void AddDigital(PlatformType platformType, bool? licensed)
        {
            foreach (Source source in TypeHelper.All<Source>())
                if (TypeHelper.Helper<SourceHelper>().IsDigital(source))
                    Add(platformType, source, licensed);
        }

        public static SuitableConsoleGames SuitableFor(ConsoleGeneration generation, ConsoleModel model, FirmwareType firmware)
        {
            SuitableConsoleGames result = new();

            bool isOfficial = firmware == FirmwareType.Official;
            bool? officialOrNull = isOfficial ? true : null;

            switch (generation)
            {
                case ConsoleGeneration.PS5:
                    result.Add(PlatformType.PS5, officialOrNull);
                    result.Add(PlatformType.PS4, officialOrNull);
                    break;
                case ConsoleGeneration.PS4:
                    result.Add(PlatformType.PS4, officialOrNull);
                    break;
                case ConsoleGeneration.PSVita:
                    result.Add(PlatformType.PSVita, officialOrNull);

                    if (firmware == FirmwareType.Custom)
                    {
                        result.Add(PlatformType.PSP, Source.Torrent, false);
                        result.Add(PlatformType.PSOne, Source.Torrent, false);
                    }
                    break;
                case ConsoleGeneration.PS3:
                    result.Add(PlatformType.PS3, officialOrNull);

                    if (model == ConsoleModel.PS3Fat)
                    {
                        result.Add(PlatformType.PS2, Source.Physical, true);
                        result.Add(PlatformType.PSOne, Source.Physical, true);
                    }

                    if (firmware == FirmwareType.Custom)
                        result.Add(PlatformType.PSOne, Source.Torrent, false);

                    break;
                case ConsoleGeneration.PSP:
                    if (model == ConsoleModel.PSPGO)
                        result.AddDigital(PlatformType.PSP, officialOrNull);
                    else
                        result.Add(PlatformType.PSP, officialOrNull);

                    if (firmware == FirmwareType.Custom)
                        result.Add(PlatformType.PSOne, Source.Torrent, false);
                    break;
                case ConsoleGeneration.PS2:
                    result.Add(PlatformType.PS2, officialOrNull);
                    result.Add(PlatformType.PSOne, Source.Physical, officialOrNull);

                    if (firmware == FirmwareType.Custom)
                        result.Add(PlatformType.PSOne, Source.Torrent, false);

                    break;
            }

            return result;
        }
    }
}