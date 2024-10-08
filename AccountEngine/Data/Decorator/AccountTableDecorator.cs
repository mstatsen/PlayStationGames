using OxDAOEngine.Data.Types;
using OxLibrary;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Fields;

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
                AccountField.Avatar => OxImageBoxer.BoxingImage(Dao.Image, new Size(70, 40)),
                AccountField.Consoles => ConsolesCount(),
                AccountField.Games => GamesCount(),
                AccountField.StrategeLink => StrategeLink,
                AccountField.PSNProfilesLink => PSNProfilesLink,
                _ => base.Value(field)
            };
        }

        private object? StrategeLink =>
            Link("Stratege");

        private object? PSNProfilesLink =>
            Link("PSNProfiles");

        private object? Link(string Name) =>
            Dao.Links.Find(l => l.Name.Equals(Name));

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