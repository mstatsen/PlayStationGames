using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.ControlFactory.Controls;
using PlayStationGames.ConsoleEngine.Data.Types;
using OxDAOEngine.Data.Types;

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
    }
}