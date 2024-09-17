using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxDAOEngine.ControlFactory.Controls;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Controls
{
    public class AccountsControl : ListItemsControl<ConsoleAccounts, ConsoleAccount, AccountEditor, ConsoleField, PSConsole> 
    {
        protected override string GetText() => "Accounts";
    }
}