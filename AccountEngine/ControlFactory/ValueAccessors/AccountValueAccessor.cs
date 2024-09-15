using OxLibrary.Controls;
using OxXMLEngine.ControlFactory.ValueAccessors;
using OxXMLEngine.Data;
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

        private OxComboBox ComboBox => (OxComboBox)Control;
        public override object? GetValue() =>
            ComboBox.SelectedItem == null ? null : ((Account)ComboBox.SelectedItem).Id;

        public override void SetValue(object? value)
        {
            if (value == null ||
                value is not Guid id)
                return;

            ComboBox.SelectedItem = id == Guid.Empty
                ? NullAccount
                : (object?)DataManager.ListController<AccountField, Account>()
                        .FullItemsList.Find((a) => a.Id == id);
        }
    }
}