using OxDAOEngine.Data.Types;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.Data.Decorator
{
    internal class AccountCardDecorator : AccountTableDecorator
    {
        public AccountCardDecorator(Account dao) : base(dao) { }

        public string AccountTypeText => 
            $"{TypeHelper.Name(Dao.Type)} account{(Dao.DefaultAccount ? ", default" : string.Empty)}";

        public override object? Value(AccountField field) => 
            field switch
            {
                AccountField.Type => AccountTypeText,
                AccountField.Consoles => $"{Dao.ConsolesCount} consoles",
                AccountField.Games => $"{Dao.GamesCount} games",
                _ => base.Value(field),
            };
    }
}