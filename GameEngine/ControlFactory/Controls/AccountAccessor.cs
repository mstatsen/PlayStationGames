using OxXMLEngine.ControlFactory.Accessors;
using OxXMLEngine.ControlFactory.Context;
using OxXMLEngine.ControlFactory.ValueAccessors;
using OxXMLEngine.Data;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.GameEngine.ControlFactory.ValueAccessors;
using System.Security.Principal;

namespace PlayStationGames.GameEngine.ControlFactory.Controls
{
    public class AccountAccessor<TField, TDAO> : ComboBoxAccessor<TField, TDAO>
        where TField : notnull, Enum
        where TDAO : RootDAO<TField>, new()
    {
        public AccountAccessor(IBuilderContext<TField, TDAO> context) : base(context) { }

        protected override ValueAccessor CreateValueAccessor() =>
            new AccountValueAccessor();

        protected override void InitControl()
        {
            base.InitControl();
            ComboBox.Items.Clear();
            ComboBox.Items.Add(AccountValueAccessor.NullAccount);
            RootListDAO<AccountField, Account> accountList = DataManager.ListController<AccountField, Account>().FullItemsList;

            foreach (var account in accountList)
                ComboBox.Items.Add(account);

            ComboBox.SelectedItem = AccountValueAccessor.NullAccount;
        }
    }
}
