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
                AccountFieldGroup.Property,
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
                AccountFieldGroup.Property => "Property",
                AccountFieldGroup.System => "System",
                _ => string.Empty,
            };

        public override AccountFieldGroup Group(AccountField field) =>
            field switch
            {
                AccountField.StrategeLink or 
                AccountField.PSNProfilesLink => 
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
                AccountFieldGroup.Auth,
                AccountFieldGroup.Links,
                AccountFieldGroup.Property
            }.Contains(group);

        public override int DefaultGroupHeight(AccountFieldGroup group) => 60;

    }
}