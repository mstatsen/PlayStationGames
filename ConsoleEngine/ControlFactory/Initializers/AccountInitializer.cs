using OxLibrary.Controls;
using OxLibrary.Interfaces;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.ConsoleEngine.Data;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Initializers;

public class AccountInitializer : ComboBoxInitializer
{
    public ListDAO<ConsoleAccount>? ExistingAccounts;

    public override void InitControl(IOxControl control)
    {
        OxPicturedComboBox<Account> ComboBox = (OxPicturedComboBox<Account>)control;

        if (ComboBox.Items.Count > 0)
            ComboBox.SelectedIndex = 0;
    }

    public override bool AvailableValue(object value) =>
        ExistingAccounts is not null 
        && value is Account account 
        && !ExistingAccounts.Contains(a => a.Id.Equals(account.Id));
}