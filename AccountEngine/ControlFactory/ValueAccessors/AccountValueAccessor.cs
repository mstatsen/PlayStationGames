using OxLibrary.Controls;
using OxDAOEngine.ControlFactory.ValueAccessors;
using OxDAOEngine.Data;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.ControlFactory.ValueAccessors
{
    public class AccountValueAccessor : ValueAccessor
    {
        public static Account NullAccount = new()
        {
            Id = Guid.Empty,
            Name = "Not available"
        };

        private OxPicturedComboBox<Account> ComboBox => (OxPicturedComboBox<Account>)Control;
        public override object? GetValue() =>
            ComboBox.SelectedItem?.Id;

        public override void SetValue(object? value)
        {
            if (value == null ||
                value is not Guid id)
                return;

            ComboBox.SelectedItem = id == Guid.Empty
                ? NullAccount
                : DataManager.ListController<AccountField, Account>()
                    .FullItemsList.Find((a) => a.Id == id);
        }
    }
}