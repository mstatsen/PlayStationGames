using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Editor;
using OxLibrary;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;

namespace PlayStationGames.AccountEngine.Editor
{
    public class AccountEditorLayoutsGenerator
        : EditorLayoutsGenerator<AccountField, Account, AccountFieldGroup>
    {
        public AccountEditorLayoutsGenerator(FieldGroupFrames<AccountField, AccountFieldGroup> groupFrames,
            ControlLayouter<AccountField, Account> layouter) : base(groupFrames, layouter) { }

        public override List<AccountField> ControlsWithoutLabel() =>
            new()
            {
                AccountField.Avatar,
                AccountField.DefaultAccount,
                AccountField.Links
            };

        public override List<AccountField> AutoSizeFields() =>
            new()
            {
                AccountField.DefaultAccount   
            };

        public override List<AccountField> OffsettingFields() =>
            new()
            {
                AccountField.Type,
                AccountField.Password,
                AccountField.Country,
            };

        public override OxWidth Top(AccountField field) =>
            field switch
            {
                AccountField.DefaultAccount =>
                    OxWh.Div(OxWh.Sub(Parent(field).Height, Height(field)), OxWh.W2),
                _ => OxWh.W8
            };

        public override OxWidth Left(AccountField field) =>
            field is AccountField.Avatar or 
                AccountField.DefaultAccount
                ? OxWh.W8
                : field is AccountField.Name or
                    AccountField.Country or
                    AccountField.Type
                    ? OxWh.W169
                    : OxWh.W74;

        public override OxWidth Width(AccountField field) =>
            field switch
            {
                AccountField.Avatar =>
                    OxWh.W80,
                AccountField.Name or
                AccountField.Country or
                AccountField.Type =>
                    OxWh.W225,
                AccountField.Login or
                AccountField.Password =>
                    OxWh.W320,
                _ => OxWh.W210
            };

        public override List<AccountField> FillDockFields() =>
            new()
            {
                AccountField.Links
            };

        public override Color BackColor(AccountField field) =>
            field switch
            {
                AccountField.Avatar =>
                    GroupFrames[AccountFieldGroup.Base].BaseColor,
                AccountField.DefaultAccount => Color.Transparent,
                _ =>
                    Color.FromArgb(250, 250, 250),
            };

        public override OxWidth Height(AccountField field) =>
            field is AccountField.Avatar
                ? OxWh.W80
                : field is AccountField.DefaultAccount 
                    ? OxWh.W20
                    : base.Height(field);

        public override AnchorStyles Anchors(AccountField field)
        {
            AnchorStyles anchors = base.Anchors(field);

            switch (field)
            {
                case AccountField.DefaultAccount:
                    anchors |= AnchorStyles.Bottom;
                    break;
            }

            return anchors;
        }

        public override List<AccountField> TitleAccordionFields() => new()
        {
            AccountField.Name
        };

        public override AccountField BackColorField => AccountField.Type;
    }
}