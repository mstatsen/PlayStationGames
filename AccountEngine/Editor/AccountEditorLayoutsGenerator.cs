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

        protected override List<AccountField> ControlsWithoutLabel() =>
            new()
            {
                AccountField.Consoles,
                AccountField.Games,
                AccountField.Avatar,
                AccountField.DefaultAccount,
                AccountField.Links
            };

        protected override List<AccountField> AutoSizeFields() =>
            new()
            {
                AccountField.DefaultAccount   
            };

        protected override List<AccountField> OffsettingFields() =>
            new()
            {
                AccountField.Type,
                AccountField.Password,
                AccountField.Country,
                AccountField.StrategeLink
            };

        protected override int Top(AccountField field) =>
            field switch
            {
                AccountField.DefaultAccount =>
                    (Parent(field).Height - Height(field)) / 2,
                _ => 8
            };

        protected override int Left(AccountField field) =>
            field is AccountField.Avatar or 
                AccountField.DefaultAccount
            ? 8
            : field is AccountField.Name or
                AccountField.Country or
                AccountField.Type
                ? 169
                : 74;

        protected override int Width(AccountField field) =>
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
                AccountField.StrategeLink or
                AccountField.PSNProfilesLink =>
                    290,
                _ => 210
            };

        protected override List<AccountField> FillDockFields() =>
            new()
            {
                AccountField.Links
            };

        protected override Color BackColor(AccountField field) =>
            field switch
            {
                AccountField.Avatar =>
                    GroupFrames[AccountFieldGroup.Base].BaseColor,
                AccountField.DefaultAccount => Color.Transparent,
                _ =>
                    Color.FromArgb(250, 250, 250),
            };

        protected override int Height(AccountField field) =>
            field == AccountField.Avatar
                ? 80
                : field == AccountField.DefaultAccount 
                    ? 20
                    : base.Height(field);

        protected override AnchorStyles Anchors(AccountField field)
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
    }
}