using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.ControlFactory.Controls;
using PlayStationGames.ConsoleEngine.Data.Types;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Data;
using OxLibrary.Controls;
using OxLibrary;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public class AccountsControl : ListItemsControl<ConsoleAccounts, ConsoleAccount, AccountEditor, ConsoleField, PSConsole> 
    {
        protected override string GetText() => "Accounts";
        protected override string ItemName() => "Account";

        public AccountsControl() => 
            GetMaximumCount += GetMaximumCountHandler;

        private int GetMaximumCountHandler() => 
            TypeHelper.Helper<ConsoleGenerationHelper>().MaxAccountsCount(
                Context.Builder[ConsoleField.Generation].EnumValue<ConsoleGeneration>(),
                Context.Builder[ConsoleField.Firmware].EnumValue<FirmwareType>());

        protected override void InitButtons()
        {
            base.InitButtons();
            OxIconButton viewButton = CreateButton(OxIcons.Eye);
            viewButton.ToolTipText = "View the account";
            PrepareViewButton(
                viewButton,
                (s, e) => DataManager.ViewItem<AccountField, Account>(AccountField.Id, SelectedItem.Id),
                true);
        }
    }
}