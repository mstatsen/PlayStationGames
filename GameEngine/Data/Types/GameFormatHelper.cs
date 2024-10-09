using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class GameFormatHelper
        : FieldAccordingHelper<GameField, GameFormat>
    {
        public GameFormat DefaultFormat(PlatformType platformType) => 
            platformType switch
            {
                PlatformType.PS5 => GameFormat.PS5,
                PlatformType.PS4 => GameFormat.PS4,
                PlatformType.PS3 => GameFormat.PS3,
                PlatformType.PSVita => GameFormat.PSVita,
                PlatformType.PS2 => GameFormat.PS2,
                PlatformType.PSP => GameFormat.PSP,
                PlatformType.PSOne => GameFormat.PSOne,
                _ => GameFormat.Other,
            };

        public override GameFormat EmptyValue() => 
            GameFormat.Other;

        public override string GetName(GameFormat value) => 
            GetShortName(value);

        public override string GetShortName(GameFormat value) => 
            value switch
            {
                GameFormat.PS5 => "PS5",
                GameFormat.PS4 => "PS4",
                GameFormat.PS3 => "PS3",
                GameFormat.PS2 => "PS2",
                GameFormat.PSOne => "PSOne",
                GameFormat.PSVita => "PSVita",
                GameFormat.PSP => "PSP",
                GameFormat.Minis => "Minis",
                GameFormat.Emulator => "Emulator",
                _ => "Other",
            };

        public override List<GameFormat> DependedList(GameField field, object value)
        {
            switch (field)
            {
                case GameField.Platform:
                    if (value is PlatformType platformType &&
                            TypeHelper.DependedList<GameField, PlatformType>(
                                GameField.PlatformFamily, 
                                PlatformFamily.Sony)
                            .Contains(platformType)
                    )
                    {
                        List<GameFormat> result = new();

                        switch (platformType)
                        {
                            case PlatformType.PS5:
                                result.Add(GameFormat.PS5);
                                result.Add(GameFormat.PS4);
                                break;
                            case PlatformType.PS4:
                                result.Add(GameFormat.PS4);
                                result.Add(GameFormat.Emulator);
                                break;
                            case PlatformType.PS3:
                                result.Add(GameFormat.PS3);
                                result.Add(GameFormat.PS2);
                                result.Add(GameFormat.PSP);
                                result.Add(GameFormat.PSOne);
                                result.Add(GameFormat.Minis);
                                result.Add(GameFormat.Emulator);
                                break;
                            case PlatformType.PSVita:
                                result.Add(GameFormat.PSVita);
                                result.Add(GameFormat.PSOne);
                                result.Add(GameFormat.Minis);
                                result.Add(GameFormat.Emulator);
                                break;
                            case PlatformType.PS2:
                                result.Add(GameFormat.PS2);
                                result.Add(GameFormat.PSOne);
                                result.Add(GameFormat.Emulator);
                                break;
                            case PlatformType.PSP:
                                result.Add(GameFormat.PSP);
                                result.Add(GameFormat.PSOne);
                                result.Add(GameFormat.Minis);
                                result.Add(GameFormat.Emulator);
                                break;
                            case PlatformType.PSOne:
                                result.Add(GameFormat.PSOne);
                                break;
                        }

                        result.Add(GameFormat.Other);
                        return result;
                    }

                    break;
                case GameField.Licensed:
                    if (value is bool boolValue && boolValue)
                    {
                        List<GameFormat> result = base.DependedList(field, value);
                        result.Remove(GameFormat.Emulator);
                        return result;
                    }

                    break;
            }

            return base.DependedList(field, value);
        }

        public bool AvailableTrophies(GameFormat value) =>
            value switch
            {
                GameFormat.Emulator or
                GameFormat.Minis or
                GameFormat.Other or
                GameFormat.PS2 or
                GameFormat.PSOne or
                GameFormat.PSP =>
                    false,
                _ =>
                    true
            };
    }
}