using OxLibrary;
using OxDAOEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data.Decorator
{
    internal class ConsoleTableDecorator : ConsoleDecorator
    {
        public ConsoleTableDecorator(PSConsole dao) : base(dao){ }
        public override object? Value(ConsoleField field)
        {
            if (TypeHelper.IsTypeHelpered(Dao[field]))
                return TypeHelper.Name(Dao[field]);

            return field switch
            {
                ConsoleField.Icon => OxImageBoxer.BoxingImage(Dao.Icon, new(70, 40)),
                ConsoleField.Storages => StoragesCount(),
                ConsoleField.Folders => FoldersCount(),
                ConsoleField.Games => GamesCount(),
                _ => base.Value(field),
            };
        }

        private readonly ConsoleGenerationHelper generationHelper = TypeHelper.Helper<ConsoleGenerationHelper>();

        private object? GamesCount()
        {
            int result = Dao.Storages.GamesCount();
            return result == 0
                ? string.Empty
                : result;
        }

        private object? FoldersCount() => 
            generationHelper.FolderSupport(Dao.Generation) 
                ? Dao.Folders.Count.ToString() 
                : string.Empty;

        private object? StoragesCount() =>
            generationHelper.StorageSupport(Dao.Generation)
                ? Dao.Storages.Count.ToString()
                : string.Empty;
    }
}