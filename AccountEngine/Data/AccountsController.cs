using OxXMLEngine.Data;
using OxXMLEngine.Data.Types;
using OxXMLEngine.Data.Fields;
using OxXMLEngine.Data.Decorator;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.Editor;
using OxXMLEngine.Data.Sorting;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Editor;
using PlayStationGames.AccountEngine.Data.Decorator;
using PlayStationGames.AccountEngine.ControlFactory;

namespace PlayStationGames.AccountEngine.Data
{

    public class AccountsController
        : ListController<AccountField, Account, AccountFieldGroup, AccountsController>
    {
        public AccountsController() : base() { }

        protected override DAOEditor<AccountField, Account, AccountFieldGroup> CreateEditor() => new AccountEditor();
        protected override DAOWorker<AccountField, Account, AccountFieldGroup> CreateWorker() => new AccountWorker();

        public override string Name => "Accounts";

        protected override FieldHelper<AccountField> RegisterFieldHelper() =>
            TypeHelper.Register<AccountFieldHelper>();

        protected override FieldGroupHelper<AccountField, AccountFieldGroup> RegisterFieldGroupHelper() =>
            TypeHelper.Register<AccountFieldGroupHelper>();

        protected override DecoratorFactory<AccountField, Account> CreateDecoratorFactory() =>
            new AccountDecoratorFactory();

        protected override ControlFactory<AccountField, Account> CreateControlFactory() =>
            new AccountControlFactory();

        public override FieldSortings<AccountField, Account> DefaultSorting() =>
            new()
            {
                new FieldSorting<AccountField, Account>(AccountField.Name, SortOrder.Ascending)
            };

        public override bool AvailableSummary => false;
        public override bool AvailableCategories => false;
        public override bool AvailableQuickFilter => false;
        public override bool AvailableCards => false;
        public override bool AvailableIcons => false;
        public override bool AvailableBatchUpdate => false;
        public override bool AvailableCopyItems => false;
    }
}