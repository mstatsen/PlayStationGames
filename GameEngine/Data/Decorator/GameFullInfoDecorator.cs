using OxLibrary;
using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;
using OxDAOEngine;
using System.Threading.Tasks.Dataflow;

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
                GameField.Format =>
                    TypeHelper.ShortName(Dao.Format),
                GameField.Platform =>
                    TypeHelper.Name(Dao.PlatformType),
                GameField.Installations => 
                    NormalizeIfEmpty(Dao.Installations.OneColumnText()),
                GameField.RelatedGames =>
                    NormalizeIfEmpty(Dao.RelatedGames.OneColumnText()),
                GameField.Devices =>
                    NormalizeIfEmpty(Dao.Devices.OneColumnText()),
                GameField.FullGenre => 
                    FullGenre,
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