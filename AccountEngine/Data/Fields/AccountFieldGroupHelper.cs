using OxDAOEngine.Data.Fields;
using OxLibrary;

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

        public override OxWidth GroupWidth(AccountFieldGroup group) => OxWh.W400;

        public override bool IsCalcedHeightGroup(AccountFieldGroup group) =>
            new List<AccountFieldGroup>
            {
                AccountFieldGroup.Base,
                AccountFieldGroup.Auth
            }.Contains(group);

        public override OxWidth DefaultGroupHeight(AccountFieldGroup group) => 
            group switch
            {
                AccountFieldGroup.Consoles or
                AccountFieldGroup.Games =>
                    OxWh.W38,
                AccountFieldGroup.Links =>
                    OxWh.W84,
                _=>
                    OxWh.W60
            };
    }
}