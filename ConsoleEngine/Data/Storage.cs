using OxXMLEngine.Data;
using OxXMLEngine.Data.Types;
using OxXMLEngine.XML;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using System.Xml;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class Storage : DAO
    {
        public Guid Id
        {
            get => id;
            set => id = ModifyField(id, value);
        }

        public StoragePlacement Placement
        {
            get => placement;
            set => placement = ModifyField(placement, value);
        }

        public string Size
        {
            get => size;
            set => size = StringValue(ModifyField(size, value));
        }

        public string FreeSize
        {
            get => freeSize;
            set => freeSize = StringValue(ModifyField(freeSize, value));
        }

        public string Name
        {
            get => name;
            set => name = StringValue(ModifyField(name, value));
        }

        public int GameCount =>
            DataManager.FullItemsList<GameField, Game>().List.FindAll(
                (g) => g.Installations.Contains(
                    (i) => i.StorageId == Id
                )
            ).Count;

        public override void Clear()
        {
            Id = Guid.Empty;
            placement = TypeHelper.DefaultValue<StoragePlacement>();
            size = string.Empty;
            freeSize = string.Empty;
            name = string.Empty;
        }

        public override void Init() => 
            GenerateGuid();

        private void GenerateGuid() =>
            Id = Guid.NewGuid();

        protected override void LoadData(XmlElement element)
        {
            id = XmlHelper.ValueGuid(element, XmlConsts.Id);
            if (id == Guid.Empty)
                GenerateGuid();

            placement = XmlHelper.Value<StoragePlacement>(element, XmlConsts.Placement);
            size = XmlHelper.Value(element, XmlConsts.Size);
            freeSize = XmlHelper.Value(element, XmlConsts.FreeSize);
            name = XmlHelper.Value(element, XmlConsts.Name);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.Id, id);
            XmlHelper.AppendElement(element, XmlConsts.Placement, placement);
            XmlHelper.AppendElement(element, XmlConsts.Size, size);
            XmlHelper.AppendElement(element, XmlConsts.FreeSize, freeSize);
            XmlHelper.AppendElement(element, XmlConsts.Name, name);
        }

        public override string ToString() =>
            $"{Name} ({Size} Gb)";

        private Guid id = Guid.Empty;
        private StoragePlacement placement = default!;
        private string size = string.Empty;
        private string freeSize = string.Empty;
        private string name = string.Empty;
    }
}