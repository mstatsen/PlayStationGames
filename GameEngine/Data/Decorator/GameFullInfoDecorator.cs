using OxLibrary;
using OxXMLEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    internal class GameFullInfoDecorator : GameCardDecorator
    {
        public GameFullInfoDecorator(Game game) : base(game) { }

        public override object Value(GameField field) => 
            field switch
            {
                GameField.Image => Image(),
                GameField.Format => Format(),
                GameField.Platform => PlatformType(),
                GameField.Installations => CalcedInstallation(),
                GameField.RelatedGames => RelatedGames(),
                _ => NormalizeIfEmpty(base.Value(field)),
            };

        private object CalcedInstallation() =>
            NormalizeIfEmpty(Dao.Installations.OneColumnText());

        private object RelatedGames() =>
            NormalizeIfEmpty(Dao.RelatedGames.OneColumnText());

        private object PlatformType() =>
            TypeHelper.Name(Dao.PlatformType);

        private object Format() =>
            TypeHelper.ShortName(Dao.Format);

        private object Image() => 
            OxImageBoxer.BoxingImage(Dao.Image, new Size(200, 97));
    }
}