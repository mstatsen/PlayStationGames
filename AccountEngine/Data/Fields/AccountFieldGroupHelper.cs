using OxDAOEngine.Data.Fields;

namespace PlayStationGames.AccountEngine.Data.Fields
{
    public class AccountFieldGroupHelper : FieldGroupHelper<AccountField, AccountFieldGroup>
    {
        public override AccountFieldGroup EmptyValue() =>
            AccountFieldGroup.Base;

        public override List<AccountFieldGroup> EditedList() => 
            new()
            {
                AccountFieldGroup.Games,
                AccountFieldGroup.Consoles,
                AccountFieldGroup.Links,
                AccountFieldGroup.Auth,
                AccountFieldGroup.Base
            };

        public override string GetName(AccountFieldGroup value) =>
            value switch
            {
                AccountFieldGroup.Base => "Account",
                AccountFieldGroup.Links => "Links",
                AccountFieldGroup.Auth => "Auth",
                AccountFieldGroup.Consoles => "Consoles",
                AccountFieldGroup.Games => "Games",
                AccountFieldGroup.System => "System",
                _ => string.Empty,
            };

        public override AccountFieldGroup Group(AccountField field) =>
            field switch
            {
                AccountField.Links => 
                    AccountFieldGroup.Links,
                AccountField.Login or
                AccountField.Password =>
                    AccountFieldGroup.Auth,
                AccountField.DefaultAccount => 
                    AccountFieldGroup.System,
                _ =>
                    AccountFieldGroup.Base,
            };

        public override int GroupWidth(AccountFieldGroup group) => 400;

        public override bool IsCalcedHeightGroup(AccountFieldGroup group) =>
            new List<AccountFieldGroup>
            {
                AccountFieldGroup.Base,
                AccountFieldGroup.Auth
            }.Contains(group);

        public override int DefaultGroupHeight(AccountFieldGroup group) => 
            group switch
            {
                AccountFieldGroup.Consoles or
                AccountFieldGroup.Games => 
                    38,
                AccountFieldGroup.Links =>
                    84,
                _=> 
                    60
            };
    }
}