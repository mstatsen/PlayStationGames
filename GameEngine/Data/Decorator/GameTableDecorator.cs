using OxLibrary;
using OxXMLEngine;
using OxXMLEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    internal class GameTableDecorator : GameDecorator
    {
        public GameTableDecorator(Game game) : base(game) {}

        public override object? Value(GameField field) => 
            field switch
            {
                GameField.Image => Image(),
                GameField.Source => SourceType(),
                GameField.CriticScore => CriticScore(),
                GameField.FullPlatinum => CalcedPlatinum(),
                GameField.FullGold => CalcedGold(),
                GameField.FullSilver => CalcedSilver(),
                GameField.FullBronze => CalcedBronze(),
                GameField.FullFromDLC => CalcedFromDLC(),
                GameField.FullNet => CalcedNet(),
                GameField.Year => Year(),
                GameField.Pegi => Pegi(),
                GameField.Installations => Installations(),
                GameField.Difficult => Difficult(),
                GameField.CompleteTime => CompleteTime(),
                GameField.FullGenre => CalcedGenre(),
                GameField.Format => Format(),
                GameField.Platform => PlatformType(),
                GameField.Progress => CalcedProgress(),
                GameField.Status => CalcedStatus(),
                GameField.StrategeLink => StrategeLink,
                GameField.PSNProfilesLink => PSNProfilesLink,
                GameField.RelatedGames => RelatedGames(),
                GameField.ReleasePlatforms => ReleasePlatforms(),
                GameField.Dlcs => DLC(),
                GameField.GameModes => GameModes(),
                GameField.TrophysetAccess => TrophysetAccesibility(),
                GameField.Verified => Verified(),
                _ => base.Value(field),
            };

        private object Verified() =>
            Dao.Verified 
                ? Consts.Yes
                : string.Empty;

        private static string CalcedTrophies(int earned, int available) =>
            available > 0
                ? $"{earned} / {available}"
                : string.Empty;

        private string CalcedNet() =>
            CalcedTrophies(Dao.EarnedTrophies.Net, Dao.AvailableTrophies.Net);

        private string Installations() =>
            Dao.Installations.Count > 0
                ? Dao.Installations.Count.ToString()
                : string.Empty;

        private string CalcedGenre()
        {
            string genre = Dao.Genre;

            if (genre.Trim() == string.Empty)
                genre = Consts.Short_Unknown;

            return $"{TypeHelper.ShortName(Dao.ScreenView)} {genre}";
        }

        private object GameModes() =>
            Dao.GameModes.ToString();

        private object? TrophysetAccesibility() =>
            TypeHelper.Name(Dao.TrophysetAccessibility);

        private object ReleasePlatforms() =>
            Dao.ReleasePlatforms.ToString();

        private object DLC() =>
            Dao.Dlcs.Count > 0
                ? Dao.Dlcs.First?.Name.Trim().ToUpper() == "All DlC".ToUpper()
                    ? "All DLC"
                    : Dao.Dlcs.Count.ToString()
                : string.Empty;

        private object RelatedGames() =>
            Dao.RelatedGames.Count > 0
                ? Dao.RelatedGames.Count.ToString()
                : string.Empty;

        private object Image() =>
            OxImageBoxer.BoxingImage(Dao.Image, new Size(70, 40));

        private object CalcedFromDLC() =>
            CalcedTrophies(Dao.EarnedTrophies.FromDLC, Dao.AvailableTrophies.FromDLC);

        private object CalcedBronze() =>
            CalcedTrophies(Dao.EarnedTrophies.Bronze, Dao.AvailableTrophies.Bronze);

        private object CalcedSilver() =>
            CalcedTrophies(Dao.EarnedTrophies.Silver, Dao.AvailableTrophies.Silver);

        private object CalcedGold() =>
            CalcedTrophies(Dao.EarnedTrophies.Gold, Dao.AvailableTrophies.Gold);

        private object CalcedPlatinum() =>
            CalcedTrophies(Dao.EarnedTrophies.Platinum, Dao.AvailableTrophies.Platinum);

        private object? Pegi() =>
            Dao.Pegi == TypeHelper.EmptyValue<Pegi>()
                ? string.Empty
                : TypeHelper.Name(Dao.Pegi);

        private object? SourceType() =>
            TypeHelper.Name(Dao.SourceType);

        private object? CalcedStatus() =>
            TypeHelper.Name(new GameCalculations(Dao).GetGameStatus());

        private object? CompleteTime() =>
            Dao.CompleteTime == TypeHelper.EmptyValue<CompleteTime>()
                ? string.Empty
                : TypeHelper.FullName(Dao.CompleteTime);

        private object? Format() =>
            Dao.Format != TypeHelper.Helper<GameFormatHelper>().DefaultFormat(Dao.PlatformType)
                ? TypeHelper.ShortName(Dao.Format)
                : string.Empty;

        private object? PlatformType() =>
            TypeHelper.ShortName(Dao.PlatformType);

        private object? Difficult() =>
            Dao.Difficult == TypeHelper.EmptyValue<DifficultRank>()
                ? string.Empty
                : TypeHelper.FullName(Dao.Difficult);

        private object CalcedProgress() =>
            Dao.TrophysetAccessibility == TrophysetAccessibility.NoSet
                ? string.Empty
                : $"{new GameCalculations(Dao).GetGameProgress()}%";

        private string CriticScore() =>
            Dao.CriticScore == GameConsts.Empty_CriticScore
            ? Consts.Short_Unknown
            : Dao.CriticScore.ToString();

        private string Year() =>
            Dao.Year == GameConsts.Empty_Year
                ? string.Empty
                : Dao.Year.ToString();

        private object? StrategeLink =>
            Link("Stratege");

        private object? PSNProfilesLink =>
            Link("PSNProfiles");

        private object? Link(string Name) =>
            Dao.Links.Find(l => l.Name.Equals(Name));
    }
}