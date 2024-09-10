using OxXMLEngine.Data;
using OxXMLEngine.Data.Types;
using OxXMLEngine.XML;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;
using System.Xml;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class Accessory : DAO
    {
        public Guid Id
        {
            get => id;
            set => id = ModifyValue(id, value);
        }

        public AccessoryType Type
        {
            get => type;
            set => type = ModifyValue(type, value);
        }

        public Color Color
        {
            get => color;
            set => color = ModifyValue(color, value);
        }

        public string Description
        {
            get => description;
            set => description = StringValue(ModifyValue(description, value));
        }

        public int Count
        {
            get => count;
            set => count = IntValue(ModifyValue(count, value));
        }

        public override void Clear()
        {
            Id = Guid.Empty;
            type = TypeHelper.DefaultValue<AccessoryType>();
            color = default;
            description = string.Empty;
            count = 1;
        }

        public override void Init() => 
            GenerateGuid();

        private void GenerateGuid() =>
            Id = Guid.NewGuid();

        protected override void LoadData(XmlElement element)
        {
            /*
            id = XmlHelper.ValueGuid(element, XmlConsts.Id);
            if (id == Guid.Empty)
                GenerateGuid();

            placement = XmlHelper.Value<StoragePlacement>(element, XmlConsts.Placement);
            size = XmlHelper.Value(element, XmlConsts.Size);
            freeSize = XmlHelper.Value(element, XmlConsts.FreeSize);
            name = XmlHelper.Value(element, XmlConsts.Name);
            */
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            /*
            XmlHelper.AppendElement(element, XmlConsts.Id, id);
            XmlHelper.AppendElement(element, XmlConsts.Placement, placement);
            XmlHelper.AppendElement(element, XmlConsts.Size, size);
            XmlHelper.AppendElement(element, XmlConsts.FreeSize, freeSize);
            XmlHelper.AppendElement(element, XmlConsts.Name, name);
            */
        }

        public override string ToString() =>
            $"{Count} {Type} (Color Gb)";

        private Guid id = Guid.Empty;
        private AccessoryType type = default!;
        private Color color = default!;
        private string description = string.Empty;
        private int count = 1;
    }
}