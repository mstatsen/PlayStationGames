using OxLibrary.Controls;
using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.ConsoleEngine.Data;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Initializers
{
    public class AccountInitializer : ComboBoxInitializer
    {
        public ListDAO<ConsoleAccount>? ExistingAccounts;

        public override void InitControl(Control control)
        {
            OxComboBox ComboBox = (OxComboBox)control;

            if (ComboBox.Items.Count > 0)
                ComboBox.SelectedIndex = 0;
        }

        public override bool AvailableValue(object value)
        {
            if (ExistingAccounts != null)
            {
                if (value is Account account)
                    return !ExistingAccounts.Contains(a => a.Id == account.Id);
            }

            return false;
        }
    }
}