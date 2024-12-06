using OxLibrary.BitmapWorker;
using OxDAOEngine;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Decorator;
using OxDAOEngine.Data.Types;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    internal class GameTableDecorator : SimpleDecorator<GameField, Game>
    {
        public GameTableDecorator(Game game) : base(game) {}

        public override object? Value(GameField field) => 
            field switch
            {
                GameField.Image => Image,
                GameField.Source => SourceType,
                GameField.CriticScore => CriticScore,
                GameField.Year => Year,
                GameField.Pegi => Pegi,
                GameField.Installations => Installations,
                GameField.Difficult => Difficult,
                GameField.CompleteTime => CompleteTime,
                GameField.Genre => CalcedGenre,
                GameField.Format => Format,
                GameField.Platform => PlatformType,
                GameField.RelatedGames => RelatedGames,
                GameField.ReleasePlatforms => ReleasePlatforms,
                GameField.Dlcs => DLC,
                GameField.TrophysetType => TrophysetAccesibility,
                GameField.Owner => Owner,
                GameField.Devices => Dao.Devices.ShortString,
                GameField.MaximumPlayers => Dao.CoachMultiplayer ? Dao.MaximumPlayers : string.Empty,
                _ => base.Value(field),
            };

        private string Installations => 
            Dao.Installations.ToString();

        private string CalcedGenre
        {
            get
            {
                string genre = Dao.GenreName;

                if (genre.Trim().Equals(string.Empty))
                    genre = Consts.Short_Unknown;

                return $"{TypeHelper.ShortName(Dao.ScreenView)} {genre}";
            }
        }

        private object? TrophysetAccesibility =>
            TypeHelper.Name(Dao.Trophyset.Type);

        private object ReleasePlatforms =>
            Dao.ReleasePlatforms.ToString();

        private object DLC =>
            Dao.Dlcs.Count > 0
                ? Dao.Dlcs.First!.Name.Trim().ToUpper().Equals("All DlC".ToUpper())
                    ? "All DLC"
                    : Dao.Dlcs.Count.ToString()
                : string.Empty;

        private object RelatedGames =>
            Dao.RelatedGames.Count > 0
                ? Dao.RelatedGames.Count.ToString()
                : string.Empty;

        private object Image =>
            OxBitmapWorker.BoxingImage(Dao.Image, new(70, 40));

        private object? Pegi =>
            Dao.Pegi.Equals(TypeHelper.EmptyValue<Pegi>())
                ? string.Empty
                : TypeHelper.Name(Dao.Pegi);

        private object? SourceType =>
            TypeHelper.Name(Dao.SourceType);

        private object? CompleteTime =>
            Dao.Trophyset.CompleteTime.Equals(TypeHelper.EmptyValue<CompleteTime>())
                ? string.Empty
                : TypeHelper.Name(Dao.Trophyset.CompleteTime);

        private object? Format =>
            Dao.Format.Equals(TypeHelper.Helper<GameFormatHelper>().DefaultFormat(Dao.PlatformType))
                ? string.Empty
                : TypeHelper.ShortName(Dao.Format);

        private object? PlatformType =>
            TypeHelper.ShortName(Dao.PlatformType);

        private object? Difficult =>
            Dao.Trophyset.Difficult.Equals(TypeHelper.EmptyValue<Difficult>())
                ? string.Empty
                : TypeHelper.Name(Dao.Trophyset.Difficult);

        private string CriticScore =>
            Dao.CriticScore is GameConsts.Empty_CriticScore
            ? Consts.Short_Unknown
            : Dao.CriticScore.ToString();

        private string Year =>
            Dao.Year is GameConsts.Empty_Year
                ? string.Empty
                : Dao.Year.ToString();

        public object Owner
        {
            get
            {
                Account? account = DataManager.ListController<AccountField, Account>().FullItemsList.
                    Find((a) => a.Id.Equals(Dao.Owner));

                if (account is not null)
                    return account.Name;

                return string.Empty;
            }
        }
    }
}