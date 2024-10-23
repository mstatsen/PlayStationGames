using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Grid;
using OxDAOEngine.View;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data.Types;
using PlayStationGames.AccountEngine.Grid;
using PlayStationGames.AccountEngine.View;

namespace PlayStationGames.AccountEngine.ControlFactory
{
    public class AccountControlFactory : ControlFactory<AccountField, Account>
    {
        protected override IControlAccessor? CreateOtherAccessor(IBuilderContext<AccountField, Account> context) => 
            context is FieldContext<AccountField, Account> accessorContext
                ? accessorContext.Field switch
                {
                    AccountField.Type =>
                        new EnumAccessor<AccountField, Account, AccountType>(context),
                    _ => base.CreateOtherAccessor(context),
                }
                : base.CreateOtherAccessor(context);

        public override IItemInfo<AccountField, Account> CreateInfoCard() =>
            new AccountFullInfoCard();

        public override IItemCard<AccountField, Account>? CreateCard(ItemViewMode viewMode) => 
            new AccountCard(viewMode);

        public override GridPainter<AccountField, Account> CreateGridPainter(
            GridFieldColumns<AccountField> columns, GridUsage usage) => new AccountGridPainter(columns);

        protected override ItemColorer<AccountField, Account> CreateItemColorer() =>
            new AccountColorer();
    }
}