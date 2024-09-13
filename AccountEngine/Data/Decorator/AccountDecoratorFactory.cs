using OxXMLEngine.Data.Decorator;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.Data.Decorator
{
    public class AccountDecoratorFactory : DecoratorFactory<AccountField, Account>
    {
        protected override Decorator<AccountField, Account> Simple(Account dao) =>
            new AccountDecorator(dao);

        protected override Decorator<AccountField, Account> Table(Account dao) => 
            new AccountTableDecorator(dao);

        protected override Decorator<AccountField, Account> FullInfo(Account dao) =>
            new AccountFullInfoDecorator(dao);

        protected override Decorator<AccountField, Account> HTML(Account dao) =>
            new AccountHtmlDecorator(dao);
    }
}