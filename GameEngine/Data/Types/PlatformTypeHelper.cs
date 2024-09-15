using OxXMLEngine;
using OxXMLEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class PlatformTypeHelper
        : FieldAccordingHelper<GameField, PlatformType>
    {
        public override string GetFullName(PlatformType type) => 
            TypeHelper.Name(DependsOnValue(type)) + " " + GetName(type);

        public bool IsPSNPlatform(PlatformType type) =>
            PlatformWithTrophies(type)
                || type == PlatformType.PSP;

        public bool PlatformWithTrophies(PlatformType type) =>
            type == PlatformType.PS3
                || type == PlatformType.PS4
                || type == PlatformType.PS5
                || type == PlatformType.PSVita;

        public override object DependsOnValue(PlatformType value) => value switch
        {
            PlatformType.PS5 or 
            PlatformType.PS4 or 
            PlatformType.PS3 or 
            PlatformType.PSVita or 
            PlatformType.PS2 or 
            PlatformType.PSP or 
            PlatformType.PSOne => 
                PlatformFamily.Sony,
            PlatformType.XBoxXS or 
            PlatformType.XBoxOne or 
            PlatformType.XBox360 or 
            PlatformType.XBox or 
            PlatformType.Win => 
                PlatformFamily.Microsoft,
            PlatformType.NSwitch or 
            PlatformType.NWiiU or 
            PlatformType.NWii or 
            PlatformType.N3DS or
            PlatformType.NGB or
            PlatformType.NGBC or
            PlatformType.NGBA =>
                PlatformFamily.Nintendo,
            PlatformType.iOS or 
            PlatformType.Android => 
                PlatformFamily.Mobile,
            _ => 
                (object)PlatformFamily.Other,
        };

        public override string GetName(PlatformType value) => 
            value switch
            {
                PlatformType.PS5 => "PlayStation 5",
                PlatformType.PS4 => "PlayStation 4",
                PlatformType.PS3 => "PlayStation 3",
                PlatformType.PSVita => "PlayStation Vita",
                PlatformType.PS2 => "PlayStation 2",
                PlatformType.PSP => "PlayStation Portable",
                PlatformType.PSOne => "PlayStation 1",
                PlatformType.XBoxXS => "XBox XS",
                PlatformType.XBoxOne => "XBox One",
                PlatformType.XBox360 => "XBox 360",
                PlatformType.XBox => "XBox",
                PlatformType.Win => "Windows",
                PlatformType.NSwitch => "Switch",
                PlatformType.NWiiU => "Wii U",
                PlatformType.NWii => "Wii",
                PlatformType.N3DS => "3ds",
                PlatformType.NGB => "GameBoy",
                PlatformType.NGBC => "GameBoy Color",
                PlatformType.NGBA => "GameBoy Advanced",
                PlatformType.iOS => "iOS",
                PlatformType.Android => "Android",
                _ => "Unknown",
            };

        public override string GetShortName(PlatformType value) => 
            value switch
            {
                PlatformType.PS5 => "PS5",
                PlatformType.PS4 => "PS4",
                PlatformType.PS3 => "PS3",
                PlatformType.PSVita => "PSVita",
                PlatformType.PS2 => "PS2",
                PlatformType.PSP => "PSP",
                PlatformType.PSOne => "PS1",
                PlatformType.XBoxXS => "XBoxXS",
                PlatformType.XBoxOne => "XBoxOne",
                PlatformType.XBox360 => "XBox360",
                PlatformType.XBox => "XBox",
                PlatformType.Win => "Win",
                PlatformType.NSwitch => "NS",
                PlatformType.NWiiU => "WiiU",
                PlatformType.NWii => "Wii",
                PlatformType.N3DS => "3ds",
                PlatformType.NGB => "GB",
                PlatformType.NGBC => "GBC",
                PlatformType.NGBA => "GBA",
                PlatformType.iOS => "iOS",
                PlatformType.Android => "Android",
                _ => Consts.Short_Unknown,
            };

        public override PlatformType EmptyValue() => 
            PlatformType.Other;
        public override PlatformType DefaultValue() => 
            PlatformType.PS4;

        public override List<PlatformType> DependedList(GameField field, object value)
        {
            switch (field)
            {
                case GameField.PlatformFamily:
                    if (value is PlatformFamily family)
                    {
                        List<PlatformType> result = new();

                        foreach (PlatformType platform in All())
                            if (DependsOnValue<PlatformFamily>(platform) == family)
                                result.Add(platform);

                        return result;
                    }
                    break;
            }

            return base.DependedList(field, value);
        }

        public bool StoragesSupport(PlatformType platformType) => 
            platformType != PlatformType.PSOne;

        public List<ConsoleGeneration> Generations(PlatformType platformType, Source source, bool licensed)
        {
            List<ConsoleGeneration> result = new();

            bool isDigital = TypeHelper.Helper<SourceHelper>().IsDigital(source);

            switch (platformType)
            {
                case PlatformType.PS5:
                    result.Add(ConsoleGeneration.PS5);
                    break;
                case PlatformType.PS4:
                    result.Add(ConsoleGeneration.PS5);
                    result.Add(ConsoleGeneration.PS4);
                    break;
                case PlatformType.PS3:
                    result.Add(ConsoleGeneration.PS3);
                    break;
                case PlatformType.PSVita:
                    result.Add(ConsoleGeneration.PSVita);
                    break;
                case PlatformType.PS2:
                    result.Add(ConsoleGeneration.PS2);
                    break;
                case PlatformType.PSP:
                    result.Add(ConsoleGeneration.PSP);

                    if (!licensed && !isDigital)
                    {
                        result.Add(ConsoleGeneration.PS2);
                        result.Add(ConsoleGeneration.PSVita);
                    }
                    break;

                case PlatformType.PSOne:
                    result.Add(ConsoleGeneration.PS1);
                    result.Add(ConsoleGeneration.PS2);
                    result.Add(ConsoleGeneration.PS3);

                    if (!licensed && !isDigital)
                    {
                        result.Add(ConsoleGeneration.PSP);
                        result.Add(ConsoleGeneration.PSVita);
                    }

                    break;
            }
            return result;
        }

        public override Color GetBaseColor(PlatformType value) => default;
        public override Color GetFontColor(PlatformType value) => default;
    }
}