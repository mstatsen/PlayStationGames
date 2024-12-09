using OxLibrary;
using OxLibrary.Controls;
using OxLibrary.Interfaces;
using OxLibrary.Panels;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.ControlFactory.Accessors;
using OxDAOEngine.ControlFactory.Context;
using OxDAOEngine.ControlFactory.ValueAccessors;
using OxDAOEngine.Data;
using PlayStationGames.AccountEngine.ControlFactory.ValueAccessors;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using OxLibrary.Geometry;


namespace PlayStationGames.AccountEngine.ControlFactory.Accessors;

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

        if (Context.Scope is not ControlScope.Editor 
            || Parameters is null 
            || Parameters.UseNullable)
            ComboBox.Items.Add(AccountValueAccessor.NullAccount);

        if (Context.Scope is not ControlScope.Editor || 
            Parameters is null 
            || !Parameters.OnlyNullable)
        {
            foreach (var account in DataManager.ListController<AccountField, Account>().FullItemsList)
                if (AvailableValue(account))
                    ComboBox.Items.Add(account);
        }

        SetDefaultValue();
    }

    public override void SetDefaultValue()
    {
        if (Parameters is not null 
            && Parameters.UseNullable)
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

    private OxTextBox ReadOnlyLabel = default!;
    private readonly OxPicture ReadOnlyPicture = new();

    private readonly short ReadOnlyPictureSize = 24;

    protected override IOxControl? CreateReadOnlyControl()
    {
        OxPanel readOnlyControl = new()
        {
            Height = ReadOnlyPictureSize
        };
        ReadOnlyLabel = (OxTextBox)base.CreateReadOnlyControl()!;
        ReadOnlyLabel.Parent = readOnlyControl;
        ReadOnlyLabel.AutoSize = true;
        ReadOnlyLabel.Left = ReadOnlyPictureSize;
        ReadOnlyPicture.Parent = readOnlyControl;
        ReadOnlyPicture.Height = ReadOnlyPictureSize;
        ReadOnlyPicture.Width = ReadOnlyPictureSize;
        ReadOnlyPicture.MinimumSize = new(ReadOnlyPictureSize, ReadOnlyPictureSize);
        ReadOnlyPicture.Top = 0;
        ReadOnlyPicture.Left = 0;
        ReadOnlyPicture.Dock = OxDock.Left;
        ReadOnlyPicture.PictureSize = ReadOnlyPictureSize;
        return readOnlyControl;
    }

    protected override void OnControlSizeChanged()
    {
        base.OnControlSizeChanged();

        if (ReadOnlyControl is not null)
        {
            ReadOnlyControl.Width = OxSH.Add(ReadOnlyPictureSize, ReadOnlyLabel.Width);
            ReadOnlyControl.Height = ReadOnlyPictureSize;
            ReadOnlyLabel.Top = OxSH.Half(ReadOnlyControl.Height - ReadOnlyLabel.Height);
        }
    }

    protected override void OnControlLocationChanged()
    {
        base.OnControlLocationChanged();

        if (ReadOnlyControl is not null)
            ReadOnlyControl.Top -= 2;
    }

    protected override void OnControlFontChanged()
    {
        base.OnControlFontChanged();
        ReadOnlyLabel.Font = ReadOnlyControl!.Font;
    }

    protected override void OnControlTextChanged(string? text)
    {
        if (ReadOnlyControl is null)
            return;

        ReadOnlyLabel.Text = Control.Text;
        OnControlFontChanged();
    }

    protected override void OnControlBackColorChanged()
    {
        if (ReadOnlyControl is null)
            return;

        base.OnControlBackColorChanged();

        ReadOnlyLabel.BackColor = ReadOnlyControl.BackColor;
    }
}