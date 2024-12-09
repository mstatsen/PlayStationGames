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

        public override short Top(AccountField field) =>
            field switch
            {
                AccountField.DefaultAccount =>
                    (short)((Parent(field).Height - Height(field)) / 2),
                _ => 8
            };

        public override short Left(AccountField field) =>
            (short)(field is AccountField.Avatar or
                AccountField.DefaultAccount
                ? 8
                : field is AccountField.Name or
                    AccountField.Country or
                    AccountField.Type
                    ? 169
                    : 74);

        public override short Width(AccountField field) =>
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

        public override short Height(AccountField field) =>
            (short)(field is AccountField.Avatar
                ? 80
                : field is AccountField.DefaultAccount 
                    ? 20
                    : base.Height(field));

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