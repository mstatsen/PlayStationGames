using OxDAOEngine.Data.Decorator;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.Data.Decorator
{
    internal class AccountDecorator : Decorator<AccountField, Account>
    {
        public AccountDecorator(Account dao) : base(dao){ }

        public override object? Value(AccountField field) => Dao[field];
    }
}