using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxDAOEngine.XML;
using PlayStationGames.GameEngine.Data.Types;
using System.Xml;

namespace PlayStationGames.GameEngine.Data
{
    public class Device : DAO
    {
        private DeviceType type;

        public DeviceType Type
        {
            get => type;
            set => type = ModifyValue(type, value);
        }

        public override void Clear() => 
            type = TypeHelper.DefaultValue<DeviceType>();

        public override void Init() { }

        protected override void SaveData(XmlElement element, bool clearModified = true) => 
            XmlHelper.AppendElement(element, XmlConsts.Device, Type);

        protected override void LoadData(XmlElement element) => 
            type = TypeHelper.Parse<DeviceType>(element.InnerText);

        public override bool Equals(object? obj) => 
            obj is Device otherDevice
            && (base.Equals(obj) 
                || Type.Equals(otherDevice.Type));

        public override int GetHashCode() => 
            2049151605 + Type.GetHashCode();

        public override string ToString() =>
            TypeHelper.Name(Type);

        public Device() : base() =>
            WithoutXmlNode = true;

        public override int CompareTo(DAO? other) => 
            other is Device otherDevice 
                ? Type.CompareTo(otherDevice.Type) 
                : -1;

        public override string ShortString => TypeHelper.ShortName(type);
    }
}