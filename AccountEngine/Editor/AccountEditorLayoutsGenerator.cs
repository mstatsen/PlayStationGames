using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data.Fields;
using OxXMLEngine.Editor;
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
                AccountField.Avatar
            };

        protected override List<AccountField> OffsettingFields() =>
            new()
            {
                AccountField.Password,
                AccountField.Country,
                AccountField.StrategeLink
            };

        protected override int Top(AccountField field) => 8;

        protected override int Left(AccountField field) =>
            field == AccountField.Avatar
            ? 8
            : field is AccountField.PSNProfilesLink or 
                AccountField.StrategeLink
                ? 84 
                : field is AccountField.Name or
                    AccountField.Country
                    ? 154
                    : 74;

        protected override int Width(AccountField field) =>
            field switch
            {
                AccountField.Avatar => 
                    80,
                AccountField.Name or
                AccountField.Country => 
                    220,
                AccountField.Login or
                AccountField.Password => 
                    296,
                AccountField.StrategeLink or
                AccountField.PSNProfilesLink =>
                    288,
                _ => 210
            };

        protected override Color BackColor(AccountField field) =>
            field switch
            {
                AccountField.Avatar =>
                    GroupFrames[AccountFieldGroup.Base].BaseColor,
                _ =>
                    Color.FromArgb(250, 250, 250),
            };

        protected override int Height(AccountField field) =>
            field == AccountField.Avatar
                ? 80
                : base.Height(field);
    }
}