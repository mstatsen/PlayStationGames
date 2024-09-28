using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.ControlFactory.ValueAccessors;
using OxDAOEngine.Data;
using OxLibrary.Controls;
using OxLibrary.Panels;
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

        protected override void AfterControlsCreated()
        {
            base.AfterControlsCreated();
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

        protected override void OnControlValueChanged(object? value)
        {
            if (value is Guid accountId)
                value = DataManager.Item<AccountField, Account>(AccountField.Id, accountId);

            base.OnControlValueChanged(value);

            if (value is Account account)
                ReadOnlyPicture.Image = account.Image;

            OnControlSizeChanged();
        }

        private OxLabel ReadOnlyLabel = default!;
        private readonly OxPicture ReadOnlyPicture = new();

        private readonly int ReadOnlyPictureSize = 24;

        protected override Control? CreateReadOnlyControl()
        {
            OxPane readOnlyControl = new()
            {
                Height = ReadOnlyPictureSize
            };
            ReadOnlyLabel = (OxLabel)base.CreateReadOnlyControl()!;
            ReadOnlyLabel.Parent = readOnlyControl;
            ReadOnlyLabel.AutoSize = true;
            ReadOnlyLabel.Left = ReadOnlyPictureSize;
            ReadOnlyPicture.Parent = readOnlyControl;
            ReadOnlyPicture.Height = ReadOnlyPictureSize;
            ReadOnlyPicture.Width = ReadOnlyPictureSize;
            ReadOnlyPicture.MinimumSize = new Size(ReadOnlyPictureSize, ReadOnlyPictureSize);
            ReadOnlyPicture.Top = 0;
            ReadOnlyPicture.Left = 0;
            ReadOnlyPicture.Dock = DockStyle.Left;
            ReadOnlyPicture.PictureSize = ReadOnlyPictureSize;
            return readOnlyControl;
        }

        protected override void OnControlSizeChanged()
        {
            base.OnControlSizeChanged();

            if (ReadOnlyControl != null)
            {
                ReadOnlyControl.Width = ReadOnlyPictureSize + ReadOnlyLabel.Width;
                ReadOnlyControl.Height = ReadOnlyPictureSize;
                ReadOnlyLabel.Top = (ReadOnlyControl.Height - ReadOnlyLabel.Height) / 2;
            }
        }

        protected override void OnControlLocationChanged()
        {
            base.OnControlLocationChanged();

            if (ReadOnlyControl != null)
                ReadOnlyControl.Top -= 2;
        }

        protected override void OnControlFontChanged()
        {
            base.OnControlFontChanged();
            ReadOnlyLabel.Font = ReadOnlyControl!.Font;
        }

        protected override void OnControlTextChanged(string? text)
        {
            if (ReadOnlyControl == null)
                return;

            ReadOnlyLabel.Text = Control.Text;
            OnControlFontChanged();
        }
    }
}