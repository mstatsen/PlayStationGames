using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxDAOEngine.XML;
using PlayStationGames.GameEngine.Data.Types;
using System.Xml;

namespace PlayStationGames.GameEngine.Data
{
    public class Platform : DAO
    {
        private PlatformType type;

        public PlatformType Type
        {
            get => type;
            set => type = ModifyValue(type, value);
        }

        public override void Clear() => 
            type = TypeHelper.DefaultValue<PlatformType>();

        public override void Init() { }

        protected override void SaveData(XmlElement element, bool clearModified = true) => 
            XmlHelper.AppendElement(element, XmlConsts.Platform, Type);

        protected override void LoadData(XmlElement element) => 
            type = TypeHelper.Parse<PlatformType>(element.InnerText);

        public override bool Equals(object? obj) => 
            obj is Platform otherPlatform 
            && (base.Equals(obj) 
                || Type.Equals(otherPlatform.Type));

        public override int GetHashCode() => 
            2049151605 + Type.GetHashCode();

        public override string ToString() =>
            TypeHelper.ShortName(Type);

        public Platform() : base() =>
            WithoutXmlNode = true;

        public Platform(PlatformType platformType) : this() =>
            type = platformType;
    }
}
