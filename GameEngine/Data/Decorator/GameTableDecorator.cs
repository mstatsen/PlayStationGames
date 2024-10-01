using OxLibrary;
using OxDAOEngine;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
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
                GameField.Image => Image,
                GameField.Source => SourceType,
                GameField.CriticScore => CriticScore,
                GameField.FullPlatinum => CalcedPlatinum,
                GameField.FullGold => CalcedGold,
                GameField.FullSilver => CalcedSilver,
                GameField.FullBronze => CalcedBronze,
                GameField.FullFromDLC => CalcedFromDLC,
                GameField.Year => Year,
                GameField.Pegi => Pegi,
                GameField.Installations => Installations,
                GameField.Difficult => Difficult,
                GameField.CompleteTime => CompleteTime,
                GameField.FullGenre => CalcedGenre,
                GameField.Format => Format,
                GameField.Platform => PlatformType,
                GameField.Progress => CalcedProgress,
                GameField.Status => CalcedStatus,
                GameField.StrategeLink => StrategeLink,
                GameField.PSNProfilesLink => PSNProfilesLink,
                GameField.RelatedGames => RelatedGames,
                GameField.ReleasePlatforms => ReleasePlatforms,
                GameField.Dlcs => DLC,
                GameField.GameModes => GameModes,
                GameField.TrophysetType => TrophysetAccesibility,
                GameField.Owner => Owner,
                _ => base.Value(field),
            };

        private static string CalcedTrophies(int earned, int available) =>
            available > 0
                ? $"{earned} / {available}"
                : string.Empty;

        private string Installations => 
            Dao.Installations.ToString();

        private string CalcedGenre
        {
            get
            {
                string genre = Dao.Genre;

                if (genre.Trim() == string.Empty)
                    genre = Consts.Short_Unknown;

                return $"{TypeHelper.ShortName(Dao.ScreenView)} {genre}";
            }
        }

        private object GameModes =>
            Dao.GameModes.ToString();

        private object? TrophysetAccesibility =>
            TypeHelper.Name(Dao.Trophyset.Type);

        private object ReleasePlatforms =>
            Dao.ReleasePlatforms.ToString();

        private object DLC =>
            Dao.Dlcs.Count > 0
                ? Dao.Dlcs.First?.Name.Trim().ToUpper() == "All DlC".ToUpper()
                    ? "All DLC"
                    : Dao.Dlcs.Count.ToString()
                : string.Empty;

        private object RelatedGames =>
            Dao.RelatedGames.Count > 0
                ? Dao.RelatedGames.Count.ToString()
                : string.Empty;

        private object Image =>
            OxImageBoxer.BoxingImage(Dao.Image, new Size(70, 40));

        private object CalcedFromDLC =>
            CalcedTrophies(Dao.Trophyset.Earned.FromDLC, Dao.Trophyset.Available.FromDLC);

        private object CalcedBronze =>
            CalcedTrophies(Dao.Trophyset.Earned.Bronze, Dao.Trophyset.Available.Bronze);

        private object CalcedSilver =>
            CalcedTrophies(Dao.Trophyset.Earned.Silver, Dao.Trophyset.Available.Silver);

        private object CalcedGold =>
            CalcedTrophies(Dao.Trophyset.Earned.Gold, Dao.Trophyset.Available.Gold);

        private object CalcedPlatinum =>
            CalcedTrophies(Dao.Trophyset.Earned.Platinum, Dao.Trophyset.Available.Platinum);

        private object? Pegi =>
            Dao.Pegi == TypeHelper.EmptyValue<Pegi>()
                ? string.Empty
                : TypeHelper.Name(Dao.Pegi);

        private object? SourceType =>
            TypeHelper.Name(Dao.SourceType);

        private object? CalcedStatus =>
            TypeHelper.Name(new GameCalculations(Dao).GetGameStatus());

        private object? CompleteTime =>
            Dao.Trophyset.CompleteTime == TypeHelper.EmptyValue<CompleteTime>()
                ? string.Empty
                : TypeHelper.FullName(Dao.Trophyset.CompleteTime);

        private object? Format =>
            Dao.Format != TypeHelper.Helper<GameFormatHelper>().DefaultFormat(Dao.PlatformType)
                ? TypeHelper.ShortName(Dao.Format)
                : string.Empty;

        private object? PlatformType =>
            TypeHelper.ShortName(Dao.PlatformType);

        private object? Difficult =>
            Dao.Trophyset.Difficult == TypeHelper.EmptyValue<Difficult>()
                ? string.Empty
                : TypeHelper.FullName(Dao.Trophyset.Difficult);

        private object CalcedProgress =>
            Dao.Trophyset.Type == TrophysetType.NoSet
                ? string.Empty
                : $"{new GameCalculations(Dao).GetGameProgress()}%";

        private string CriticScore =>
            Dao.CriticScore == GameConsts.Empty_CriticScore
            ? Consts.Short_Unknown
            : Dao.CriticScore.ToString();

        private string Year =>
            Dao.Year == GameConsts.Empty_Year
                ? string.Empty
                : Dao.Year.ToString();

        private object? StrategeLink =>
            Link("Stratege");

        private object? PSNProfilesLink =>
            Link("PSNProfiles");

        public object Owner
        {
            get
            {
                Account? account = DataManager.ListController<AccountField, Account>().FullItemsList.Find((a) => a.Id == Dao.Owner);
                if (account != null)
                    return account.Name;

                return string.Empty;
            }
        }

        private object? Link(string Name) =>
            Dao.Links.Find(l => l.Name.Equals(Name));
    }
}