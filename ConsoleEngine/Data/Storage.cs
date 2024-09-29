using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxDAOEngine.XML;
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
            set => id = ModifyValue(id, value);
        }

        public StoragePlacement Placement
        {
            get => placement;
            set => placement = ModifyValue(placement, value);
        }

        public string Size
        {
            get => size;
            set => size = StringValue(ModifyValue(size, value));
        }

        public string FreeSize
        {
            get => freeSize;
            set => freeSize = StringValue(ModifyValue(freeSize, value));
        }

        public string Name
        {
            get => name;
            set => name = StringValue(ModifyValue(name, value));
        }

        public List<Game> Games =>
            DataManager.FullItemsList<GameField, Game>().List.FindAll(
                (g) => g.Installations.Contains(
                    (i) => i.StorageId == Id
                )
            );

        public int GameCount =>
            Games.Count;

        public override void Clear()
        {
            id = Guid.NewGuid();
            placement = TypeHelper.DefaultValue<StoragePlacement>();
            size = string.Empty;
            freeSize = string.Empty;
            name = string.Empty;
        }

        protected override void LoadData(XmlElement element)
        {
            id = XmlHelper.ValueGuid(element, XmlConsts.Id, true);
            placement = XmlHelper.Value<StoragePlacement>(element, XmlConsts.Placement);
            size = XmlHelper.Value(element, XmlConsts.Size);
            freeSize = XmlHelper.Value(element, XmlConsts.FreeSize);
            name = XmlHelper.Value(element, XmlConsts.Name);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.Id, id);
            XmlHelper.AppendElement(element, XmlConsts.Placement, placement);
            XmlHelper.AppendElement(element, XmlConsts.Size, size, true);
            XmlHelper.AppendElement(element, XmlConsts.FreeSize, freeSize, true);
            XmlHelper.AppendElement(element, XmlConsts.Name, name);
        }

        public override bool Equals(object? obj) => 
            base.Equals(obj)
            || (obj is Storage otherStorage
                && Id.Equals(otherStorage.Id));

        public override string ToString() =>
            $"{Name} ({Size} Gb)";

        private Guid id = Guid.Empty;
        private StoragePlacement placement = default!;
        private string size = string.Empty;
        private string freeSize = string.Empty;
        private string name = string.Empty;

        public override int GetHashCode() => 
            id.GetHashCode();

        public override void Init() { }
    }
}