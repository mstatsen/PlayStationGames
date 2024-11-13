using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.ControlFactory.Controls;
using PlayStationGames.ConsoleEngine.Data.Types;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Data;
using OxLibrary.Controls;
using OxLibrary;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls.Accounts
{
    public class AccountsControl : ListItemsControl<ConsoleAccounts, ConsoleAccount, AccountEditor, ConsoleField, PSConsole>
    {
        protected override string GetText() => "Accounts";
        protected override string ItemName() => "Account";

        public AccountsControl() =>
            GetMaximumCount += GetMaximumCountHandler;

        private int GetMaximumCountHandler() =>
            OwnerDAO == null
                ? -1
                : TypeHelper.Helper<ConsoleGenerationHelper>().MaxAccountsCount(
                    OwnerDAO.Generation,
                    OwnerDAO.Firmware);

        protected override void InitButtons()
        {
            base.InitButtons();
            OxIconButton viewButton = CreateButton(OxIcons.Eye);
            viewButton.ToolTipText = "View the account";
            PrepareViewButton(
                viewButton,
                (s, e) => 
                {
                    if (SelectedItem != null)
                        DataManager.ViewItem<AccountField, Account>(AccountField.Id, SelectedItem.Id);
                },
                true);
        }
    }
}