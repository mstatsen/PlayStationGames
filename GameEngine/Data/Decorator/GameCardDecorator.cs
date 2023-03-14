using OxLibrary;
using OxXMLEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    internal class GameCardDecorator : GameTableDecorator
    {
        public GameCardDecorator(Game game) : base(game) { }

        public override object? Value(GameField field)
        {
            return field switch
            {
                GameField.Image => Image(),
                GameField.Platform => PlatformType(),
                GameField.Progress => CalcedProgress(),
                GameField.Installations => CalcedInstallation(),
                GameField.Dlcs => Dlcs(),
                _ => base.Value(field),
            };
        }

        private object Dlcs() =>
            Dao.Dlcs.ToString();

        private object CalcedInstallation() => 
            Dao.Installations.ToString();

        private object PlatformType() =>
            TypeHelper.ShortName(Dao.PlatformType)
                + (Dao.Format == TypeHelper.Helper<GameFormatHelper>().DefaultFormat(Dao.PlatformType)
                    ? string.Empty
                    : $" / {TypeHelper.ShortName(Dao.Format)}");

        private object? CalcedProgress() =>
            new GameCalculations(Dao).AvailableTrophiesExist()
                ? base.Value(GameField.Progress)
                : "Trophies\nunavailable";

        private object? Image() =>
            OxImageBoxer.BoxingImage(Dao.Image, new Size(140, 80));
    }
}