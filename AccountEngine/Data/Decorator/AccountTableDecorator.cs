using OxXMLEngine.Data.Types;
using OxLibrary;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.Data.Decorator
{
    internal class AccountTableDecorator : AccountDecorator
    {
        public AccountTableDecorator(Account dao) : base(dao){ }
        public override object? Value(AccountField field)
        {
            if (TypeHelper.IsTypeHelpered(Dao[field]))
                return TypeHelper.Name(Dao[field]);

            return field switch
            {
                AccountField.Avatar => OxImageBoxer.BoxingImage(Dao.Avatar, new Size(70, 40)),
                AccountField.Consoles => ConsolesCount(),
                AccountField.Games => GamesCount(),
                _ => base.Value(field),
            };
        }

        private int ConsolesCount()
        {
            return 0;
        }

        private object? GamesCount()
        {
            /*TODO:
            int result = Dao.Storages.GamesCount();
            return result == 0
                ? string.Empty
                : result;
            */
            return 0;
        }
    }
}