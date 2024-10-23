using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Decorator;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Editor;
using OxDAOEngine.Data.Sorting;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Editor;
using PlayStationGames.AccountEngine.Data.Decorator;
using PlayStationGames.AccountEngine.ControlFactory;
using PlayStationGames.AccountEngine.Data.Types;
using OxLibrary;
using OxDAOEngine.Data.Links;
using OxDAOEngine.Grid;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;

namespace PlayStationGames.AccountEngine.Data
{

    public class AccountsController
        : ListController<AccountField, Account, AccountFieldGroup, AccountsController>
    {
        public AccountsController() : base() { }

        protected override DAOEditor<AccountField, Account, AccountFieldGroup> CreateEditor() => new AccountEditor();
        protected override DAOWorker<AccountField, Account, AccountFieldGroup> CreateWorker() => new AccountWorker();

        public override string ListName => "PSN Accounts";
        public override string ItemName => "Account";

        protected override FieldHelper<AccountField> RegisterFieldHelper() =>
            TypeHelper.Register<AccountFieldHelper>();

        protected override void RegisterHelpers()
        {
            TypeHelper.Register<AccountLinkTypeHelper>();
            TypeHelper.Register<AccountTypeHelper>();
        }

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

        protected override void BeforeLoad()
        {
            base.BeforeLoad();
            OnAfterLoad += SortAfterLoad;
        }

        private void SortAfterLoad(object? sender, EventArgs e) => 
            FullItemsList.Sort(DefaultSorting()?.SortingsList);

        protected override Bitmap? GetIcon() => OxIcons.Account;
        public override bool AvailableSummary => false;
        public override bool AvailableCategories => false;
        public override bool AvailableQuickFilter => false;
        public override bool AvailableCards => false;
        public override bool AvailableIcons => false;
        public override bool AvailableBatchUpdate => false;
        public override bool AvailableCopyItems => false;
        public override bool UseImageList => true;

        public override List<ToolStripMenuItem>? MenuItems(Account? item)
        {
            if (item == null)
                return null;

            List<ToolStripMenuItem> result = new();

            if (item.Links.Count > 0)
            {
                ToolStripMenuItem goToItem = new("Go to", OxIcons.Go);

                foreach (Link<AccountField> link in item.Links)
                    goToItem.DropDownItems.Add(new ItemsRootGridLinkToolStripMenuItem<AccountField>(link));

                result.Add(goToItem);
            }

            return result;
        }
    }
}