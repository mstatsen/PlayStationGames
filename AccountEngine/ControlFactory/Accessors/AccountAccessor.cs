using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.ControlFactory.ValueAccessors;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxLibrary.Controls;
using PlayStationGames.AccountEngine.ControlFactory.ValueAccessors;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.ControlFactory.Accessors
{
    public class AccountAccessor<TField, TDAO> : ComboBoxAccessor<TField, TDAO, Account, OxPicturedComboBox<Account>>
        where TField : notnull, Enum
        where TDAO : RootDAO<TField>, new()
    {
        public AccountAccessor(IBuilderContext<TField, TDAO> context) : base(context) { }

        protected override ValueAccessor CreateValueAccessor() =>
            new AccountValueAccessor();

        protected override void AfterControlCreated()
        {
            base.AfterControlCreated();
            ComboBox.GetItemPicture += GetAccountPictureHandler;
        }

        private Bitmap? GetAccountPictureHandler(Account item) => item.Image;

        private AccountAccessorParameters? Parameters =>
            Context!.AdditionalContext is AccountAccessorParameters
                ? Context!.AdditionalContext! as AccountAccessorParameters
                : null;

        protected override void InitControl()
        {
            base.InitControl();
            ComboBox.Items.Clear();

            if (Context.IsQuickFilter)
                ComboBox.Items.Add(Account.AnyAccount);

            if (Context.Scope != ControlScope.Editor ||
                Parameters == null ||
                Parameters.UseNullable)
                ComboBox.Items.Add(AccountValueAccessor.NullAccount);

            if (Context.Scope != ControlScope.Editor || 
                Parameters == null ||
                !Parameters.OnlyNullable)
            {
                foreach (var account in DataManager.ListController<AccountField, Account>().FullItemsList)
                    if (AvailableValue(account))
                        ComboBox.Items.Add(account);
            }

            SetDefaultValue();
        }

        public override void SetDefaultValue()
        {
            if (Parameters != null &&
                Parameters.UseNullable)
                base.SetDefaultValue();
        }
    }
}
