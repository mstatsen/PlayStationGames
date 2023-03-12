using OxXMLEngine.Data;
using OxXMLEngine.XML;
using System.Xml;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class Folder : DAO
    {
        public string Name
        {
            get => name;
            set => name = StringValue(ModifyField(name, value));
        }

        public override void Clear() => 
            name = string.Empty;

        public override void Init() { }

        protected override void LoadData(XmlElement element) => 
            name = XmlHelper.Value(element, XmlConsts.Name);

        protected override void SaveData(XmlElement element, bool clearModified = true) => 
            XmlHelper.AppendElement(element, XmlConsts.Name, name);

        private string name = string.Empty;

        public override string? ToString() =>
            Name;

        public override bool Equals(object? obj) =>
            obj is Folder otherFolder
            && (base.Equals(obj)
                || Name.Equals(otherFolder.Name));

        public override int GetHashCode() =>
            539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
    }
}
