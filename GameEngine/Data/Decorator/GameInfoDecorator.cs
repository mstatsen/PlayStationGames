using OxLibrary.BitmapWorker;
using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    internal class GameInfoDecorator : GameCardDecorator
    {
        public GameInfoDecorator(Game game) : base(game) { }

        public override object Value(GameField field) => 
            field switch
            {
                GameField.Image =>
                    OxBitmapWorker.BoxingImage(Dao.Image, new(200, 97)),
                GameField.Format =>
                    TypeHelper.ShortName(Dao.Format),
                GameField.Installations => 
                    NormalizeIfEmpty(Dao.Installations.OneColumnText()),
                GameField.RelatedGames =>
                    NormalizeIfEmpty(Dao.RelatedGames.OneColumnText()),
                GameField.AppliesTo =>
                    NormalizeIfEmpty(Dao.Trophyset.AppliesTo),
                GameField.Devices =>
                    NormalizeIfEmpty(Dao.Devices.OneColumnText()),
                _ => 
                    NormalizeIfEmpty(base.Value(field)),
            };
    }
}