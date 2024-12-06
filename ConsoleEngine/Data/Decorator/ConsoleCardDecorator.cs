using OxDAOEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxLibrary.BitmapWorker;


namespace PlayStationGames.ConsoleEngine.Data.Decorator
{
    internal class ConsoleCardDecorator : ConsoleTableDecorator
    {
        public ConsoleCardDecorator(PSConsole dao) : base(dao){ }

        public object InstalledGames 
        {
            get
            {
                int gamesCount = Dao.GamesCount;

                return gamesCount is 0 
                    ? string.Empty 
                    : gamesCount is 1 
                        ? "1 game" 
                        : (object)$"{Dao.GamesCount} games";
            }
        }

        public override object? Value(ConsoleField field) => 
            TypeHelper.IsTypeHelpered(Dao[field])
                ? TypeHelper.Name(Dao[field])
                : field switch
                    {
                        ConsoleField.Icon =>
                            OxBitmapWorker.BoxingImage(Dao.Icon, new(80, 33)),
                        ConsoleField.Storages =>
                            NormalizeIfEmpty(Dao.Storages.ShortString),
                        ConsoleField.Accessories =>
                            NormalizeIfEmpty(Dao.Accessories.ShortColumnText()),
                        ConsoleField.Games =>
                            InstalledGames,
                        _ =>
                            base.Value(field),
                    };
    }
}