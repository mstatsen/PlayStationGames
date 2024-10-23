using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Editor;
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

        public override int Top(AccountField field) =>
            field switch
            {
                AccountField.DefaultAccount =>
                    (Parent(field).Height - Height(field)) / 2,
                _ => 8
            };

        public override int Left(AccountField field) =>
            field is AccountField.Avatar or 
                AccountField.DefaultAccount
                ? 8
                : field is AccountField.Name or
                    AccountField.Country or
                    AccountField.Type
                    ? 169
                    : 74;

        public override int Width(AccountField field) =>
            field switch
            {
                AccountField.Avatar => 
                    80,
                AccountField.Name or
                AccountField.Country or
                AccountField.Type => 
                    225,
                AccountField.Login or
                AccountField.Password => 
                    320,
                _ => 210
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

        public override int Height(AccountField field) =>
            field == AccountField.Avatar
                ? 80
                : field == AccountField.DefaultAccount 
                    ? 20
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