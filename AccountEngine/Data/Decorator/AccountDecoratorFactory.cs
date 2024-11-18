using OxDAOEngine.Data.Decorator;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.Data.Decorator
{
    public class AccountDecoratorFactory : DecoratorFactory<AccountField, Account>
    {
        protected override Decorator<AccountField, Account> Table(Account dao) => 
            new AccountTableDecorator(dao);

        protected override Decorator<AccountField, Account> Info(Account dao) =>
            new AccountInfoDecorator(dao);

        protected override Decorator<AccountField, Account> Card(Account dao) =>
            new AccountCardDecorator(dao);

        protected override Decorator<AccountField, Account> HTML(Account dao) =>
            new AccountHtmlDecorator(dao);
    }
}