using OxXMLEngine.ControlFactory;
using OxXMLEngine.ControlFactory.Accessors;
using OxXMLEngine.ControlFactory.Context;
using OxXMLEngine.Grid;
using OxXMLEngine.View;

using OxXMLEngine.ControlFactory.Initializers;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
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
                    _ => base.CreateOtherAccessor(context),
                }
                : base.CreateOtherAccessor(context);

        protected override IInitializer? Initializer(IBuilderContext<AccountField, Account> context) => 
            context.Name switch
            {
                _ => base.Initializer(context),
            };

        public override IItemInfo<AccountField, Account> CreateInfoCard() =>
            new AccountFullInfoCard();

        public override GridPainter<AccountField, Account> CreateGridPainter(
            GridFieldColumns<AccountField> columns, GridUsage usage) => new AccountGridPainter(columns);

        public override IItemCard<AccountField, Account> CreateCard(ItemViewMode viewMode) => 
            new AccountCard(viewMode);

        protected override ItemColorer<AccountField, Account> CreateItemColorer() =>
            new AccountColorer();
    }
}