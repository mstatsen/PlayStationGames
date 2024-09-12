using OxXMLEngine.Data.Types;
using OxLibrary;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.Data.Decorator
{
    internal class AccountCardDecorator : AccountTableDecorator
    {
        public AccountCardDecorator(Account dao) : base(dao){ }
        public override object? Value(AccountField field)
        {
            if (TypeHelper.IsTypeHelpered(Dao[field]))
                return TypeHelper.Name(Dao[field]);

            return field switch
            {
                AccountField.Avatar =>
                    OxImageBoxer.BoxingImage(Dao.Avatar, new Size(140, 140)),
                _ => base.Value(field),
            };
        }
    }
}