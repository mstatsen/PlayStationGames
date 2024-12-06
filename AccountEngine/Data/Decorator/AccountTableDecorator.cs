using OxDAOEngine.Data.Decorator;
using OxDAOEngine.Data.Types;
using OxLibrary;
using OxLibrary.BitmapWorker;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.Data.Decorator
{
    internal class AccountTableDecorator : SimpleDecorator<AccountField, Account>
    {
        public AccountTableDecorator(Account dao) : base(dao){ }
        public override object? Value(AccountField field)
        {
            if (TypeHelper.IsTypeHelpered(Dao[field]))
                return TypeHelper.Name(Dao[field]);

            return field switch
            {
                AccountField.Avatar => 
                    OxBitmapWorker.BoxingImage(Dao.Image, new(OxWh.W70, OxWh.W40)),
                AccountField.Consoles =>
                    Dao.ConsolesCount,
                AccountField.Games => 
                    Dao.GamesCount,
                _ => base.Value(field)
            };
        }
    }
}