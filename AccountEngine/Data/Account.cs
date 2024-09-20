using OxDAOEngine.Data;
using OxDAOEngine.Data.Decorator;
using OxDAOEngine.Data.Links;
using OxDAOEngine.Data.Types;
using OxDAOEngine.XML;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data;
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

        public Guid Id
        {
            get => id;
            set => id = GuidValue(ModifyValue(AccountField.Id, id, value));
        }

        public Bitmap? Avatar
        {
            get => avatar;
            set => avatar = ModifyValue(AccountField.Avatar, avatar, value);
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

        public string Country
        {
            get => country;
            set => country = StringValue(ModifyValue(AccountField.Country, country, value));
        }

        public override void Clear()
        {
            Id = Guid.Empty;
            DefaultAccount = false;
            Avatar = null;
            avatarBase64 = string.Empty;
            Country = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
            Links.Clear();
        }

        public override void Init()
        {
            GenerateGuid();
            AddMember(AccountField.Links, Links);
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
            avatarBase64 = XmlHelper.Value(element, XmlConsts.Image);
            Avatar = XmlHelper.ValueBitmap(element, XmlConsts.Image);
            Country = XmlHelper.Value(element, XmlConsts.Country);
            Login = XmlHelper.Value(element, XmlConsts.Login);
            Password = XmlHelper.Value(element, XmlConsts.Password);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            base.SaveData(element, clearModified);
            XmlHelper.AppendElement(element, XmlConsts.Id, Id);
            XmlHelper.AppendElement(element, XmlConsts.Default, DefaultAccount);
            
            if (avatarBase64 == string.Empty)
            {
                if (Avatar != null)
                    XmlHelper.AppendElement(element, XmlConsts.Image, Avatar);
            }
            else XmlHelper.AppendElement(element, XmlConsts.Image, avatarBase64);

            XmlHelper.AppendElement(element, XmlConsts.Country, Country);
            XmlHelper.AppendElement(element, XmlConsts.Login, Login);
            XmlHelper.AppendElement(element, XmlConsts.Password, Password);
        }

        private Guid id = Guid.Empty;
        private string avatarBase64 = string.Empty;
        private Bitmap? avatar = null;
        private string country = string.Empty;
        private string login = string.Empty;
        private string password = string.Empty;
        private string psnProfilesLink = string.Empty;
        private string strategeLink = string.Empty;
        private bool defaultAccount = false;


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
                    if (value is Link<GameField> link)
                        return new Links<GameField>()
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
                case AccountField.DefaultAccount:
                    DefaultAccount = BoolValue(value);
                    break;
                case AccountField.Login:
                    Login = StringValue(value);
                    break;
                case AccountField.Avatar:
                    Avatar = (Bitmap?)value;
                    avatarBase64 = string.Empty;
                    break;
                case AccountField.Password:
                    Password = StringValue(value);
                    break;
                case AccountField.Country:
                    Country = StringValue(value);
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
                AccountField.Avatar => Avatar,
                AccountField.Id => Id,
                AccountField.Name => base.GetFieldValue(field),
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

            return Name.CompareTo(otherAccount.Name);
        }

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
    }
}