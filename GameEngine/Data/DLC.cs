using System.Xml;
using OxDAOEngine.Data;
using OxDAOEngine.XML;

namespace PlayStationGames.GameEngine.Data
{
    public class DLC : DAO
    {
        public DLC() : base() { }
            

        private string name = string.Empty;
        private bool acquired = false;
        public readonly Trophyset Trophyset = new();

        public string Name
        {
            get => name;
            set => name = StringValue(ModifyValue(name, value));
        }

        public bool Acquired
        {
            get => acquired;
            set => acquired = BoolValue(ModifyValue(acquired, value));
        }

        public override void Clear()
        {
            name = string.Empty;
            acquired = false;
            Trophyset.Clear();
        }

        public override void Init() => 
            AddMember(Trophyset);

        protected override void LoadData(XmlElement element)
        {
            name = XmlHelper.Value(element, XmlConsts.Name);

            if (name == string.Empty)
                name = element.InnerText;

            acquired = XmlHelper.ValueBool(element, XmlConsts.Aqcuired);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.Name, Name);
            XmlHelper.AppendElement(element, XmlConsts.Aqcuired, Acquired);
        }

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