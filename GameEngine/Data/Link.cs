using OxXMLEngine.Data;
using OxXMLEngine.XML;
using PlayStationGames.GameEngine.Data.Types;
using System.Xml;

namespace PlayStationGames.GameEngine.Data
{
    public class Link : DAO
    {
        private string name = string.Empty;
        private string url = string.Empty;

        public string Name
        {
            get => name;
            set => name = StringValue(ModifyValue(name, value));
        }

        public string Url
        {
            get => url;
            set => url = StringValue(ModifyValue(url, value));
        }

        public override void Clear()
        {
            Name = string.Empty;
            Url = string.Empty;
        }

        public override void Init() { }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.Name, Name);
            XmlHelper.AppendElement(element, XmlConsts.URL, Url, true);
        }

        protected override void LoadData(XmlElement element)
        {
            Name = XmlHelper.Value(element, XmlConsts.Name);
            Url = XmlHelper.Value(element, XmlConsts.URL);
        }
        public override string ToString() => 
            Name;

        public override bool Equals(object? obj) =>
            obj is Link ohterLink
            && (base.Equals(obj)
                || (Name.Equals(ohterLink.Name)
                    && Url.Equals(ohterLink.Url)));

        public override int GetHashCode() => 
            HashCode.Combine(Name, Url);

        public LinkType Type
        {
            get 
            {
                string lowerName = Name.ToLower();

                if (lowerName.Equals("stratege"))
                    return LinkType.Stratege;
                else
                if (lowerName.Equals("psnprofiles"))
                    return LinkType.PSNProfiles;
                else
                    return LinkType.Walkthrough;
            }
        }

        public override int CompareTo(DAO? other)
        {
            if (Equals(other))
                return 0;
            
            Link? otherLink = (Link?)other;

            if (otherLink == null)
                return 1;

            if (Type == LinkType.Stratege)
                return -1;
            
            if (otherLink.Type == LinkType.Stratege)
                return 1;
            
            if (Type == LinkType.PSNProfiles)
                return -1;
            
            if (otherLink.Type == LinkType.PSNProfiles)
                return 1;

            return Name.CompareTo(otherLink.Name);
        }

        public Color LinkColor => 
            Type switch
            {
                LinkType.Stratege =>
                    Color.FromArgb(180, 170, 130),
                LinkType.PSNProfiles =>
                    Color.FromArgb(130, 160, 180),
                _ =>
                    Color.FromArgb(170, 150, 200),
            };

        public override object ExtractKeyValue => Name;
    }
}