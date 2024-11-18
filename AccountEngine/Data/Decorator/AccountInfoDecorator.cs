using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.Data.Decorator
{
    internal class AccountInfoDecorator : AccountCardDecorator
    {
        public AccountInfoDecorator(Account dao) : base(dao) { }

        public override object? Value(AccountField field) =>
            field switch
            {
                AccountField.Consoles => 
                    Dao.Consoles.OneColumnText(),
                _ => 
                    base.Value(field),
            };
    }
}