using OxLibrary;
using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;
using OxDAOEngine;
using PlayStationGames.GameEngine.Data.Types;

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
                GameField.Installations => 
                    NormalizeIfEmpty(Dao.Installations.OneColumnText()),
                GameField.RelatedGames =>
                    NormalizeIfEmpty(Dao.RelatedGames.OneColumnText()),
                GameField.AppliesTo =>
                    NormalizeIfEmpty(Dao.Trophyset.AppliesTo),
                GameField.Devices =>
                    NormalizeIfEmpty(Dao.Devices.OneColumnText()),
                _ => NormalizeIfEmpty(base.Value(field)),
            };
    }
}