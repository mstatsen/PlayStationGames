using OxLibrary;
using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;
using OxDAOEngine;
using OxDAOEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    internal class GameCardDecorator : GameTableDecorator
    {
        public GameCardDecorator(Game game) : base(game) { }

        public override object? Value(GameField field)
        {
            return field switch
            {
                GameField.Image =>
                    Image(),
                GameField.Platform =>
                    PlatformAndFormat,
                GameField.Installations =>
                    Dao.Installations.OneColumnText(),
                GameField.Dlcs =>
                    Dao.Dlcs.OneColumnText(),
                GameField.Genre =>
                    FullGenre,
                GameField.Licensed =>
                    Licensed,
                GameField.Installed =>
                    Dao.Licensed
                        ? "Installed"
                        : string.Empty,
                GameField.Region =>
                    RegionAndLanguage,
                GameField.AvailableGold => 
                    Dao.GetFullTrophyset.Available.Gold,
                GameField.AvailableSilver =>
                    Dao.GetFullTrophyset.Available.Silver,
                GameField.AvailableBronze =>
                    Dao.GetFullTrophyset.Available.Bronze,
                GameField.CriticScore =>
                    Dao.Format is GameFormat.Emulator
                        ? string.Empty
                        : Dao.CriticScore,
                _ => base.Value(field),
            };
        }

        private object PlatformAndFormat =>
            TypeHelper.ShortName(Dao.PlatformType)
                + (Dao.Format.Equals(TypeHelper.Helper<GameFormatHelper>().DefaultFormat(Dao.PlatformType))
                    ? string.Empty
                    : $" / {TypeHelper.ShortName(Dao.Format)}");

        private object? Image() =>
            OxImageBoxer.BoxingImage(Dao.Image, new(140, 80));

        private object FullGenre
        {
            get
            {
                if (Dao.Format is GameFormat.Emulator)
                    return string.Empty;

                string genre = Dao.GenreName;

                if (genre.Trim().Equals(string.Empty))
                    genre = Consts.Short_Unknown;

                string player =
                    $"{(Dao.SinglePlayer ? "single, " : string.Empty)}{(Dao.Multiplayer ? $"multiplayler, " : string.Empty)}";

                if (!player.Equals(string.Empty))
                    player = player.Remove(player.Length - 2);

                player = 
                    !player.Equals(string.Empty) 
                        ? "(" + player + ")" 
                        : string.Empty;
                return $"{TypeHelper.ShortName(Dao.ScreenView)} {genre} {player}";
            }
        }

        private string RegionAndLanguage
        {
            get
            {
                if (Dao.Format is GameFormat.Emulator)
                    return string.Empty;

                string result = $"{TypeHelper.Name(Dao.GameRegion)} ({TypeHelper.Name(Dao.GameLanguage)})";
                
                if (!Dao.Code.Trim().Equals(string.Empty))
                    result += $", {Dao.Code}";

                return result;
            }
        }

        public object Licensed 
        {
            get
            {
                string result = string.Empty;

                if (Dao.Licensed)
                {
                    result += "Linesed";

                    if (!Dao.Owner.Equals(Guid.Empty))
                        result += $" ({DataManager.Item<AccountField, Account>(AccountField.Id, Dao.Owner)})";
                }

                return result;
            }
        }
    }
}