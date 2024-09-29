using OxDAOEngine.Data;
using OxDAOEngine.Data.Decorator;
using OxDAOEngine.Data.Types;
using OxDAOEngine.XML;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using System.Xml;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class PSConsole : RootDAO<ConsoleField>
    {
        public PSConsole() : base() { }

        public Guid Id
        {
            get => id;
            set => ModifyValue(ConsoleField.Id, id, value, (n) => id = GuidValue(n));
        }

        public Bitmap Icon => 
            (Bitmap)this[ConsoleField.Icon]!;

        public ConsoleGeneration Generation
        {
            get => generation;
            set => ModifyValue(ConsoleField.Generation, generation, value, n => generation = n);
        }

        public ConsoleModel Model
        {
            get => model;
            set => ModifyValue(ConsoleField.Model, model, value, n => model = n);
        }

        public FirmwareType Firmware
        {
            get => firmware;
            set => ModifyValue(ConsoleField.Firmware, firmware, value, n => firmware = n);
        }

        public Folders Folders => folders;
        public Storages Storages => storages;
        public ConsoleAccounts Accounts => accounts;
        public Accessories Accessories => accessories;

        public override void Clear()
        {
            id = Guid.NewGuid();
            generation = TypeHelper.DefaultValue<ConsoleGeneration>();
            model = TypeHelper.DefaultValue<ConsoleModel>();
            storages.Clear();
            folders.Clear();
            accounts.Clear();
            accessories.Clear();
        }

        public override void Init()
        {
            AddListMember(ConsoleField.Storages, Storages);
            AddListMember(ConsoleField.Folders, Folders);
            AddListMember(ConsoleField.Accessories, Accessories);
            AddListMember(ConsoleField.Accounts, Accounts);
        }

        protected override void LoadData(XmlElement element)
        {
            base.LoadData(element);
            id = XmlHelper.ValueGuid(element, XmlConsts.Id, true);
            generation = XmlHelper.Value<ConsoleGeneration>(element, XmlConsts.Generation);
            model = XmlHelper.Value<ConsoleModel>(element, XmlConsts.Model);
            firmware = XmlHelper.Value<FirmwareType>(element, XmlConsts.Firmware);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            base.SaveData(element, clearModified);
            XmlHelper.AppendElement(element, XmlConsts.Id, Id);
            XmlHelper.AppendElement(element, XmlConsts.Generation, generation);
            XmlHelper.AppendElement(element, XmlConsts.Model, model);
            XmlHelper.AppendElement(element, XmlConsts.Firmware, firmware);
        }

        private Guid id = Guid.Empty;
        private ConsoleGeneration generation = default!;
        private ConsoleModel model = default!;
        private readonly Storages storages = new ();
        private readonly Folders folders = new ();
        private readonly ConsoleAccounts accounts = new();
        private readonly Accessories accessories = new();
        private FirmwareType firmware;

        public List<Game> Games() =>
            DataManager.FullItemsList<GameField, Game>().List.FindAll(
                (g) => g.Installations.Contains(
                    (i) => i.ConsoleId == Id
                )
            );

        public int GamesCount => Storages.GamesCount();

        protected override void SetFieldValue(ConsoleField field, object? value)
        {
            base.SetFieldValue(field, value);

            switch (field)
            {
                case ConsoleField.Id:
                    Id = GuidValue(value);
                    break;
                case ConsoleField.Generation:
                    Generation = TypeHelper.Value<ConsoleGeneration>(value);
                    break;
                case ConsoleField.Model:
                    Model = TypeHelper.Value<ConsoleModel>(value);
                    break;
                case ConsoleField.Firmware:
                    Firmware = TypeHelper.Value<FirmwareType>(value);
                    break;
                case ConsoleField.Storages:
                    Storages.CopyFrom((DAO?)value);
                    break;
                case ConsoleField.Folders:
                    Folders.CopyFrom((DAO?)value);
                    break;
                case ConsoleField.Accessories:
                    Accessories.CopyFrom((DAO?)value);
                    break;
                case ConsoleField.Accounts:
                    Accounts.CopyFrom((DAO?)value);
                    break;
            }
        }

        protected override object? GetFieldValue(ConsoleField field) => 
            field switch
            {
                ConsoleField.Console => this,
                ConsoleField.Id => Id,
                ConsoleField.Name => base.GetFieldValue(field),
                ConsoleField.Generation => Generation,
                ConsoleField.Model => Model,
                ConsoleField.Firmware => Firmware,
                ConsoleField.Storages => Storages,
                ConsoleField.Folders => Folders,
                ConsoleField.Accounts => Accounts,
                ConsoleField.Accessories => Accessories,
                ConsoleField.Games => 
                    DataManager.DecoratorFactory<ConsoleField, PSConsole>().Decorator(DecoratorType.Table, this),
                ConsoleField.Icon => TypeHelper.Helper<ConsoleGenerationHelper>().Icon(Generation),
                _ => null,
            };

        public override int CompareTo(DAO? other)
        {
            if (Equals(other))
                return 0;
            
            PSConsole? otherConsole = (PSConsole?)other;

            if (otherConsole == null)
                return 1;

            int result = Name.CompareTo(otherConsole.Name);

            if (result != 0)
                return result;

            if (Generation > otherConsole.Generation)
                return 1;
                
            if (Generation < otherConsole.Generation)
                return -1;

            if (Model > otherConsole.Model)
                return 1;
                
            if (Model < otherConsole.Model)
                return -1;

            if (Firmware > otherConsole.Firmware)
                return 1;

            if (Firmware < otherConsole.Firmware)
                return -1;

            return 0;
        }

        public override int CompareField(ConsoleField field, IFieldMapping<ConsoleField> y)
        {
            switch (field)
            {
                case ConsoleField.Id:
                case ConsoleField.Name:
                    return StringValue(this[field]).CompareTo(StringValue(y[field]));
            }

            if (y is PSConsole yConsole)
                return field switch
                {
                    ConsoleField.Generation => generation.CompareTo(yConsole.Generation),
                    ConsoleField.Model => model.CompareTo(yConsole.Model),
                    ConsoleField.Firmware => firmware.CompareTo(yConsole.Firmware),
                    ConsoleField.Storages => storages.CompareTo(yConsole.Storages),
                    ConsoleField.Folders => folders.CompareTo(yConsole.Folders),
                    ConsoleField.Accessories => folders.CompareTo(yConsole.Accessories),
                    ConsoleField.Accounts => accounts.CompareTo(yConsole.Accounts),
                    _ => 0,
                };

            return base.CompareField(field, y);
        }

        private readonly ConsoleFieldHelper fieldHelper =
            TypeHelper.Helper<ConsoleFieldHelper>();

        public override object ParseCaldedValue(ConsoleField field, string value) => 
            value;

        public override bool IsCalcedField(ConsoleField field) =>
            fieldHelper.CalcedFields.Contains(field);

        public override string FullTitle() => Name;

        public override bool Equals(object? obj)
        {
            if (!base.Equals(obj))
                return false;
            
            if (obj is PSConsole otherConsole)
                return Generation.Equals(otherConsole.Generation)
                    && Model.Equals(otherConsole.Model)
                    && Storages.Equals(otherConsole.Storages)
                    && Folders.Equals(otherConsole.Folders)
                    && Accounts.Equals(otherConsole.Accounts)
                    && Accessories.Equals(otherConsole.Accessories)
                    && Firmware.Equals(otherConsole.Firmware);
            else return false;
        }

        public override int GetHashCode() => 
            Id.GetHashCode();
    }
}