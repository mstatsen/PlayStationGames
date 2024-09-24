using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.ControlFactory.ValueAccessors;
using OxDAOEngine.Data;
using PlayStationGames.AccountEngine.ControlFactory.ValueAccessors;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.ControlFactory.Accessors
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

            AccountAccessorParameters? parameters = Context!.AdditionalContext is AccountAccessorParameters 
                ? Context!.AdditionalContext! as AccountAccessorParameters 
                : null;

            if (parameters == null ||
                parameters.UseNullable)
                ComboBox.Items.Add(AccountValueAccessor.NullAccount);

            if (parameters == null ||
                !parameters.OnlyNullable)
            {
                foreach (var account in DataManager.ListController<AccountField, Account>().FullItemsList)
                    if (AvailableValue(account))
                        ComboBox.Items.Add(account);
            }

            SetDefaultValue();
        }

        public override void SetDefaultValue()
        {

            foreach (object item in ComboBox.Items)
            {
                if (item is Account account && account.DefaultAccount)
                {
                    ComboBox.SelectedItem = account;
                    return;
                }
            }

            if (ComboBox.Items.Count > 1)
            {
                ComboBox.SelectedIndex = 1;
                return;
            }

            base.SetDefaultValue();
        }
    }
}
