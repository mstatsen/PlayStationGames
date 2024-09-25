using OxDAOEngine.Data;
using OxDAOEngine.Data.Decorator;
using OxDAOEngine.Data.Links;
using OxDAOEngine.Data.Types;
using OxDAOEngine.XML;
using OxLibrary.Data.Countries;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data.Types;
using System.Xml;

namespace PlayStationGames.AccountEngine.Data
{
    public class Account : RootDAO<AccountField>
    {
        public Account() : base() =>
            GenerateGuid();

        public readonly Links<AccountField> Links = new();

        public bool DefaultAccount
        { 
            get => defaultAccount;
            set => defaultAccount = BoolValue(ModifyValue(AccountField.DefaultAccount, defaultAccount, value));
        }

        public AccountType Type
        {
            get => type;
            set => type = ModifyValue(AccountField.Type, type, value);
        }

        public Guid Id
        {
            get => id;
            set => id = GuidValue(ModifyValue(AccountField.Id, id, value));
        }

        public string Login
        {
            get => login;
            set => login = StringValue(ModifyValue(AccountField.Login, login, value));
        }

        public string Password
        {
            get => password;
            set => password = StringValue(ModifyValue(AccountField.Password, password, value));
        }

        public Country? Country
        {
            get => country;
            set => country = ModifyValue(AccountField.Country, country, value);
        }

        public override void Clear()
        {
            base.Clear();
            Id = Guid.Empty;
            DefaultAccount = false;
            Country = null;
            Login = string.Empty;
            Password = string.Empty;
            Type = default!;
            Links.Clear();
        }

        public override void Init()
        {
            GenerateGuid();
            AddListMember(AccountField.Links, Links);
        }

        private void GenerateGuid() =>
            Id = Guid.NewGuid();

        protected override void LoadData(XmlElement element)
        {
            base.LoadData(element);
            id = XmlHelper.ValueGuid(element, XmlConsts.Id);
            if (id == Guid.Empty)
                GenerateGuid();
            DefaultAccount = XmlHelper.ValueBool(element, XmlConsts.Default);
            Country = CountryList.GetCountry(CountryField.Alpha3, XmlHelper.Value(element, XmlConsts.Country));
            Login = XmlHelper.Value(element, XmlConsts.Login);
            Password = XmlHelper.Value(element, XmlConsts.Password);
            Type = XmlHelper.Value<AccountType>(element, XmlConsts.Type);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            base.SaveData(element, clearModified);
            XmlHelper.AppendElement(element, XmlConsts.Id, Id);
            XmlHelper.AppendElement(element, XmlConsts.Default, DefaultAccount);
            XmlHelper.AppendElement(element, XmlConsts.Type, Type);

            if (Country != null)
                XmlHelper.AppendElement(element, XmlConsts.Country, Country.Alpha3, true);

            XmlHelper.AppendElement(element, XmlConsts.Login, Login);
            XmlHelper.AppendElement(element, XmlConsts.Password, Password);
        }

        private Guid id = Guid.Empty;
        private Country? country = null;
        private string login = string.Empty;
        private string password = string.Empty;
        private bool defaultAccount = false;
        private AccountType type = default!;


        /*
         * TODO:
        public List<Game> Games() =>
            DataManager.FullItemsList<GameField, Game>().List.FindAll(
                (g) => g.Installations.Contains(
                    (i) => i.ConsoleId == Id
                )
            );

        public List<PSConsole> Consoles() =>
            DataManager.FullItemsList<ConsoleField, PSConsole>().List.FindAll(
                (g) => g.Installations.Contains(
                    (i) => i.ConsoleId == Id
                )
            );

        //public int GamesCount => Storages.GamesCount();
        */

        private static object? PrepareValueToSet(AccountField field, object? value)
        {
            switch (field)
            {
                case AccountField.Links:
                    if (value is Link<AccountField> link)
                        return new Links<AccountField>()
                        {
                            link
                        };
                    break;
            }

            return value;
        }

        protected override void SetFieldValue(AccountField field, object? value)
        {
            value = PrepareValueToSet(field, value);

            if (!CheckValueModified(this[field], value))
                return;

            base.SetFieldValue(field, value);

            switch (field)
            {
                case AccountField.Id:
                    Id = GuidValue(value);
                    break;
                case AccountField.Type:
                    Type = TypeHelper.Value<AccountType>(value);
                    break;
                case AccountField.DefaultAccount:
                    DefaultAccount = BoolValue(value);
                    break;
                case AccountField.Login:
                    Login = StringValue(value);
                    break;
                case AccountField.Password:
                    Password = StringValue(value);
                    break;
                case AccountField.Country:
                    Country = (Country?)value;
                    break;
                case AccountField.Links:
                    Links.CopyFrom((DAO?)value);
                    break;
            }
        }

        protected override object? GetFieldValue(AccountField field) =>
            field switch
            {
                AccountField.Account => this,
                AccountField.Id => Id,
                AccountField.Name or 
                AccountField.Avatar =>
                    base.GetFieldValue(field),
                AccountField.Type => Type,
                AccountField.DefaultAccount => DefaultAccount,
                AccountField.Login => Login,
                AccountField.Password => Password,
                AccountField.Country => Country,
                AccountField.Links => Links,
                AccountField.Consoles or
                AccountField.Games =>
                    DataManager.DecoratorFactory<AccountField, Account>().Decorator(DecoratorType.Table, this),
                _ => null,
            };

        public override int CompareTo(DAO? other)
        {
            if (Equals(other))
                return 0;
            
            Account? otherAccount = (Account?)other;

            if (otherAccount == null)
                return 1;

            return Id.CompareTo(otherAccount.Id);
        }

        public override bool Equals(object? obj) => 
            base.Equals(obj)
            && (obj is Account otherAccount)
            && DefaultAccount.Equals(otherAccount.DefaultAccount)
            && Type.Equals(otherAccount.Type)
            && (Country != null
                ? Country.Equals(otherAccount.Country)
                : otherAccount.Country != null
            )
            && Login.Equals(otherAccount.Login)
            && Password.Equals(otherAccount.Password)
            && Links.Equals(otherAccount.Links);

        public override int CompareField(AccountField field, IFieldMapping<AccountField> y)
        {
            switch (field)
            {
                case AccountField.Id:
                case AccountField.Name:
                case AccountField.Country:
                case AccountField.Login:
                case AccountField.Password:
                case AccountField.PSNProfilesLink:
                case AccountField.StrategeLink:
                    return StringValue(this[field]).CompareTo(StringValue(y[field]));
            };

            if (y is Account yAccount)
                return field switch
                {
                    AccountField.Links =>
                        Links.CompareTo(yAccount.Links),
                    AccountField.Type =>
                        Type.CompareTo(yAccount.Type),
                    _ => base.CompareField(field, y),
                };

            return base.CompareField(field, y);
        }

        private readonly AccountFieldHelper fieldHelper =
            TypeHelper.Helper<AccountFieldHelper>();

        public override object ParseCaldedValue(AccountField field, string value) => 
            value;

        public override bool IsCalcedField(AccountField field) =>
            fieldHelper.CalcedFields.Contains(field);

        public override string FullTitle() => Name;

        public override int GetHashCode() => Id.GetHashCode();
    }
}