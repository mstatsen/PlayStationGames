using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Fields.Types;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Data.Filter.Types;
using OxDAOEngine.Data.Types;
using PlayStationGames.AccountEngine.Data.Types;

namespace PlayStationGames.AccountEngine.Data.Fields
{
    internal class AccountFieldsFillingDictionary : FieldsFillingDictionary<AccountField> { };
    internal class AccountFieldsVariantDictionary : Dictionary<FieldsVariant, AccountFieldsFillingDictionary> { };

    public class AccountFieldHelper : FieldHelper<AccountField>
    {
        public override AccountField EmptyValue() => AccountField.Field;

        public override string GetName(AccountField value) =>
            value switch
            {
                AccountField.Account => "Account",
                AccountField.Id => "Id",
                AccountField.Avatar => "Avatar",
                AccountField.Name => "Name",
                AccountField.Type => "Type",
                AccountField.Login => "Login",
                AccountField.Password => "Password",
                AccountField.Country => "Country",
                AccountField.Consoles => "Consoles",
                AccountField.Games => "Games",
                AccountField.StrategeLink => "Stratege",
                AccountField.Links => "Links",
                AccountField.PSNProfilesLink => "PSNProfiles",
                AccountField.DefaultAccount => "Default Account",
                _ => string.Empty,
            };

        private readonly AccountFieldsVariantDictionary fieldDictionary = new()
        {
            [FieldsVariant.QuickFilter] = new AccountFieldsFillingDictionary(),
            [FieldsVariant.QuickFilterText] = new AccountFieldsFillingDictionary(),
            [FieldsVariant.Category] = new AccountFieldsFillingDictionary(),
            [FieldsVariant.Inline] = new AccountFieldsFillingDictionary()
            {
                [FieldsFilling.Full] = new List<AccountField>
                {
                    AccountField.Type,
                    AccountField.Login,
                    AccountField.Password,
                    AccountField.Country,
                    AccountField.StrategeLink,
                    AccountField.PSNProfilesLink,
                    AccountField.DefaultAccount
                },
                [FieldsFilling.Default] = new List<AccountField>
                {
                    AccountField.Login,
                    AccountField.Country
                }
            },
            [FieldsVariant.Table] = new AccountFieldsFillingDictionary()
            {
                [FieldsFilling.Full] = new List<AccountField>
                {
                    AccountField.Avatar,
                    AccountField.Name,
                    AccountField.Type,
                    AccountField.Login,
                    AccountField.Country,
                    AccountField.PSNProfilesLink,
                    AccountField.StrategeLink,
                    AccountField.Consoles,
                    AccountField.Games,
                    AccountField.DefaultAccount
                },
                [FieldsFilling.Default] = new List<AccountField>
                {
                    AccountField.Avatar,
                    AccountField.Name,
                    AccountField.Type,
                    AccountField.Login,
                    AccountField.Country,
                    AccountField.PSNProfilesLink,
                    AccountField.StrategeLink,
                    AccountField.DefaultAccount
                },
                [FieldsFilling.Min] = new List<AccountField>
                {
                    AccountField.Avatar,
                    AccountField.Name,
                    AccountField.Type
                }
            }
        };

        public override List<AccountField> GetFieldsInternal(FieldsVariant variant, FieldsFilling filling)
        {
            List<AccountField> result = new();

            if (!fieldDictionary.TryGetValue(variant, out var fillingDictionary))
                return result;

            if (!fillingDictionary.TryGetValue(filling, out var fields))
                return result;

            result.AddRange(fields);
            return result;
        }

        protected override List<AccountField> GetMandatoryFields() => new()
        {
            AccountField.Avatar,
            AccountField.Name,
            AccountField.Type,
            AccountField.Country
        };

        protected override List<AccountField> GetGroupByFields() => new() { };

        protected override List<AccountField> GetCalcedFields() => new() 
        {
            AccountField.StrategeLink,
            AccountField.PSNProfilesLink
        };

        protected override List<AccountField> GetEditingFields() => new()
        {
            AccountField.Avatar,
            AccountField.Name,
            AccountField.Type,
            AccountField.Country,
            AccountField.Login,
            AccountField.Password,
            AccountField.Links,
            AccountField.DefaultAccount
        };

        protected override List<AccountField> GetEditedFieldsExtended() => 
            EditingFields;

        protected override List<AccountField> GetFullInfoFields() => new()
        {
            AccountField.Avatar,
            AccountField.Name,
            AccountField.Type,
            AccountField.Country,
            AccountField.Consoles,
            AccountField.Games,
            AccountField.Login,
            AccountField.Password,
            AccountField.Links,
            AccountField.DefaultAccount
        };

        protected override List<AccountField> GetCardFields() => new() { };

        protected override FilterOperation GetDefaultFilterOperation(AccountField field) =>
            field switch
            {
                AccountField.Avatar or
                AccountField.StrategeLink or
                AccountField.PSNProfilesLink or
                AccountField.Password =>
                    FilterOperation.NotBlank,
                AccountField.Name or 
                AccountField.Login or
                AccountField.Country or
                AccountField.Consoles or
                AccountField.Games or
                AccountField.Links =>
                    FilterOperation.Contains,
                _ =>
                    FilterOperation.Equals
            };

        protected override FieldFilterOperations<AccountField> GetAvailableFilterOperations() => new()
        {
            [AccountField.Avatar] = FilterOperations.UnaryOperations,
            [AccountField.Consoles] = FilterOperations.EnumOperations,
            [AccountField.Country] = FilterOperations.StringOperations,
            [AccountField.Games] = FilterOperations.EnumOperations,
            [AccountField.Links] = FilterOperations.EnumOperations,
            [AccountField.Type] = FilterOperations.EnumOperations,
            [AccountField.Id] = FilterOperations.StringOperations,
            [AccountField.Login] = FilterOperations.StringOperations,
            [AccountField.Name] = FilterOperations.StringOperations,
            [AccountField.Password] = FilterOperations.UnaryOperations,
            [AccountField.PSNProfilesLink] = FilterOperations.UnaryOperations,
            [AccountField.StrategeLink] = FilterOperations.UnaryOperations,
            [AccountField.DefaultAccount] = FilterOperations.BoolOperations
        };

        protected override List<AccountField> GetSelectQuickFilterFields() => new(){ };

        public override AccountField FieldMetaData => AccountField.Field;

        public override AccountField TitleField => AccountField.Name;

        public override AccountField UniqueField => AccountField.Id;

        public override DataGridViewContentAlignment ColumnAlign(AccountField field) =>
            field == AccountField.Name
                ? DataGridViewContentAlignment.MiddleLeft
                : DataGridViewContentAlignment.MiddleCenter;

        public override int ColumnWidth(AccountField field) => 
            field switch
            {
                AccountField.Name or
                AccountField.Login => 
                    200,
                AccountField.Avatar or 
                AccountField.Country or 
                AccountField.DefaultAccount => 
                    70,
                AccountField.Games or
                AccountField.Consoles =>
                    50,
                _ => 
                    100,
            };

        public override FieldType GetFieldType(AccountField field) =>
            field switch
            {
                AccountField.Account =>
                    FieldType.Extract,
                AccountField.Id => 
                    FieldType.Guid,
                AccountField.Avatar => 
                    FieldType.Image,
                AccountField.Country => 
                    FieldType.Country,
                AccountField.Consoles or
                AccountField.Games => 
                    FieldType.List,
                AccountField.Links =>
                    FieldType.LinkList,
                AccountField.DefaultAccount =>
                    FieldType.Boolean,
                AccountField.Type =>
                    FieldType.Enum,
                _ =>
                    FieldType.String,
            };

        public override ITypeHelper? GetHelper(AccountField field) =>
            field switch
            { 
                AccountField.Type =>
                    TypeHelper.Helper<AccountTypeHelper>(),
                _=> null
            };

        public override ILinkHelper<AccountField>? GetLinkHelper() => TypeHelper.Helper<AccountLinkTypeHelper>();

        public override AccountField ImageField => AccountField.Avatar;
    }
}