using OxLibrary;
using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;
using OxDAOEngine;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    internal class GameFullInfoDecorator : GameCardDecorator
    {
        public GameFullInfoDecorator(Game game) : base(game) { }

        public override object Value(GameField field) => 
            field switch
            {
                GameField.Image =>
                    OxImageBoxer.BoxingImage(Dao.Image, new Size(200, 97)),
                GameField.Licensed =>
                    Dao.Licensed ? "Yes" : "No",
                GameField.Format =>
                    TypeHelper.ShortName(Dao.Format),
                GameField.Platform =>
                    TypeHelper.Name(Dao.PlatformType),
                GameField.Region =>
                    $"{TypeHelper.Name(Dao.GameRegion)} ({TypeHelper.Name(Dao.GameLanguage)})",
                GameField.Installations => 
                    NormalizeIfEmpty(Dao.Installations.OneColumnText()),
                GameField.RelatedGames =>
                    NormalizeIfEmpty(Dao.RelatedGames.OneColumnText()),
                GameField.AppliesTo =>
                    NormalizeIfEmpty(Dao.Trophyset.AppliesTo),
                GameField.Devices =>
                    NormalizeIfEmpty(Dao.Devices.OneColumnText()),
                GameField.FullGenre => 
                    FullGenre,
                GameField.AvailablePlatinum when Dao.Trophyset.Available.Platinum > 0 =>
                    "Yes",
                _ => NormalizeIfEmpty(base.Value(field)),
            };

        private object FullGenre
        {
            get
            {
                string genre = Dao.GenreName;

                if (genre.Trim() == string.Empty)
                    genre = Consts.Short_Unknown;

                string player =
                    $"{(Dao.SinglePlayer ? "single, " : string.Empty)}{(Dao.CoachMultiplayer ? $"multiplayler, " : string.Empty)}{(Dao.OnlineMultiplayer ? $"online, " : string.Empty)}";

                if (player != string.Empty)
                    player = player.Remove(player.Length - 2);

                player = player != string.Empty ? "(" + player + ")" : string.Empty;

                return $"{TypeHelper.ShortName(Dao.ScreenView)} {genre} {player}";
            }
        }
    }
}