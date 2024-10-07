using OxLibrary.Controls;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data;
using PlayStationGames.AccountEngine.Data;

namespace PlayStationGames.GameEngine.ControlFactory.Initializers
{
    public class AccountInitializer : ComboBoxInitializer
    {
        public ListDAO<Account>? ExistingAccounts;

        public override void InitControl(Control control)
        {
            OxPicturedComboBox<Account> ComboBox = (OxPicturedComboBox<Account>)control;

            if (ComboBox.Items.Count > 0)
                ComboBox.SelectedIndex = 0;
        }

        public override bool AvailableValue(object value)
        {
            if (ExistingAccounts != null 
                && value is Account account)
                return !ExistingAccounts.Contains(a => a.Id == account.Id);

            return false;
        }
    }
}