using OxXMLEngine.Data;
using OxXMLEngine.XML;
using System.Xml;

namespace PlayStationGames.GameEngine.Data
{
    public class DLC : DAO
    {
        public DLC() : base() =>
            WithoutXmlNode = true;

        private string name = string.Empty;
        public string Name
        {
            get => name;
            set => name = StringValue(ModifyValue(name, value));
        }

        public override void Clear() => 
            Name = string.Empty;

        public override void Init() { }

        protected override void LoadData(XmlElement element) =>
            name = element.InnerText;

        protected override void SaveData(XmlElement element, bool clearModified = true) => 
            XmlHelper.AppendElement(element, XmlConsts.DLC, Name);

        public override string ToString() => 
            Name;

        public override bool Equals(object? obj) =>
            obj is DLC otherDLC
            && (base.Equals(obj)
                || Name.Equals(otherDLC.Name));

        public override int GetHashCode() => 
            539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
    }
}