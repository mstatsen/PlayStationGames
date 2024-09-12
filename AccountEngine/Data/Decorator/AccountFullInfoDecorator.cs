using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.Data.Decorator
{
    internal class AccountFullInfoDecorator : AccountCardDecorator
    {
        public AccountFullInfoDecorator(Account dao) : base(dao) { }

        public override object? Value(AccountField field) => 
            field switch
            {
                AccountField.Consoles => ConsolesText(),
                AccountField.Games => GamesText(),
                _ => base.Value(field),
            };

        private object? GamesText()
        {
            string result = string.Empty;

            /*
            foreach (Storage storage in Dao.Storages)
                result += $"{storage.Name} ({storage.Size} Gb, {storage.GameCount} games)\n";
            */

            return result;
        }

        private object? ConsolesText()
        {
            string result = string.Empty;

            /*
            foreach (Storage storage in Dao.Storages)
                result += $"{storage.Name} ({storage.Size} Gb, {storage.GameCount} games)\n";
            */

            return result;
        }
    }
}