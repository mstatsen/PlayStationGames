using OxDAOEngine.Data.Types;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data.Types;

namespace PlayStationGames.AccountEngine.Data.Decorator
{
    internal class AccountFullInfoDecorator : AccountCardDecorator
    {
        public AccountFullInfoDecorator(Account dao) : base(dao) { }

        public override object? Value(AccountField field) =>
            field switch
            {
                AccountField.Consoles => Dao.Consoles.OneColumnText(),
                _ => base.Value(field),
            };
    }
}