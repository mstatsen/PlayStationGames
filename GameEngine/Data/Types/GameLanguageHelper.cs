using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class GameLanguageHelper : FieldAccordingHelper<GameField, GameLanguage>
    {
        public override GameLanguage EmptyValue() =>
            GameLanguage.Russian;

        public override string GetShortName(GameLanguage value) =>
            value switch
            {
                GameLanguage.Russian => "RUS",
                GameLanguage.RussianSubtitles => "RUS Sub",
                GameLanguage.English => "ENG",
                GameLanguage.EnglishSubtitles => "ENG Sub",
                _ => "Other",
            };

        public override string GetName(GameLanguage value) =>
            ShortName(value);


        public override string GetFullName(GameLanguage value) =>
            value switch
            {
                GameLanguage.Russian => "Russian voice and subtitles",
                GameLanguage.RussianSubtitles => "Russian subtitles only",
                GameLanguage.English => "English voice and subtitles",
                GameLanguage.EnglishSubtitles => "English subtitles only",
                _ => "Other",
            };
    }
}