using OxXMLEngine.Data;
using OxXMLEngine.Data.Types;
using OxXMLEngine.XML;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using System.Xml;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class PSConsole : RootDAO<ConsoleField>
    {

        public PSConsole() : base() =>
            GenerateGuid();

        public Guid Id
        {
            get => id;
            set => id = GuidValue(ModifyValue(ConsoleField.Id, id, value));
        }

        public Bitmap Icon => 
            (Bitmap)this[ConsoleField.Icon]!;

        public string Name
        {
            get => name;
            set => name = StringValue(ModifyValue(ConsoleField.Name, name, value));
        }
        public ConsoleGeneration Generation
        {
            get => generation;
            set => generation = ModifyValue(ConsoleField.Generation, generation, value);
        }

        public ConsoleModel Model
        {
            get => model;
            set => model = ModifyValue(ConsoleField.Model, model, value);
        }

        public FirmwareType Firmware
        {
            get => firmware;
            set => firmware = ModifyValue(ConsoleField.Firmware, firmware, value);
        }

        public Folders Folders => folders;
        public Storages Storages => storages;
        public Accessories Accessories => accessories;

        public override void Clear()
        {
            Id = Guid.Empty;
            Name = string.Empty;
            generation = TypeHelper.DefaultValue<ConsoleGeneration>();
            model = TypeHelper.DefaultValue<ConsoleModel>();
            storages.Clear();
            folders.Clear();
            accessories.Clear();
        }

        public override void Init()
        {
            GenerateGuid();
            AddMember(ConsoleField.Storages, Storages);
            AddMember(ConsoleField.Folders, Folders);
            AddMember(ConsoleField.Accessories, Accessories);
        }

        private void GenerateGuid() =>
            Id = Guid.NewGuid();

        protected override void LoadData(XmlElement element)
        {
            id = XmlHelper.ValueGuid(element, XmlConsts.Id);
            if (id == Guid.Empty)
                GenerateGuid();
            name = XmlHelper.Value(element, XmlConsts.Name);
            generation = XmlHelper.Value<ConsoleGeneration>(element, XmlConsts.Generation);
            model = XmlHelper.Value<ConsoleModel>(element, XmlConsts.Model);
            firmware = XmlHelper.Value<FirmwareType>(element, XmlConsts.Firmware);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.Id, Id);
            XmlHelper.AppendElement(element, XmlConsts.Name, Name);
            XmlHelper.AppendElement(element, XmlConsts.Generation, generation);
            XmlHelper.AppendElement(element, XmlConsts.Model, model);
            XmlHelper.AppendElement(element, XmlConsts.Firmware, firmware);
        }

        private Guid id = Guid.Empty;
        private string name = string.Empty;
        private ConsoleGeneration generation = default!;
        private ConsoleModel model = default!;
        private readonly Storages storages = new ();
        private readonly Folders folders = new ();
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
            switch (field)
            {
                case ConsoleField.Id:
                    Id = GuidValue(value);
                    break;
                case ConsoleField.Name:
                    Name = StringValue(value);
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
            }
        }

        protected override object? GetFieldValue(ConsoleField field) => 
            field switch
            {
                ConsoleField.Console => this,
                ConsoleField.Id => Id,
                ConsoleField.Name => Name,
                ConsoleField.Generation => Generation,
                ConsoleField.Model => Model,
                ConsoleField.Firmware => Firmware,
                ConsoleField.Storages => Storages,
                ConsoleField.Folders => Folders,
                ConsoleField.Accessories => Accessories,
                ConsoleField.Games => 
                    DataManager.DecoratorFactory<ConsoleField, PSConsole>().Decorator(OxXMLEngine.Data.Decorator.DecoratorType.Table, this),
                ConsoleField.Icon => TypeHelper.Helper<ConsoleGenerationHelper>().Icon(Generation),
                _ => null,
            };

        public override string ToString() =>
            Name;

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
    }
}