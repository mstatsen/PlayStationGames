using OxXMLEngine.Data;
using OxXMLEngine.Data.Types;
using OxXMLEngine.XML;
using PlayStationGames.GameEngine.Data.Types;
using System.Xml;

namespace PlayStationGames.GameEngine.Data
{
    public class Trophy : DAO
    {
        private TrophyType type;
        private int count;

        public TrophyType Type
        {
            get => type;
            set => type = ModifyValue(type, value);
        }

        public int Count
        {
            get => count;
            set => count = ModifyValue(count, value);
        }

        public override void Clear()
        {
            type = TypeHelper.EmptyValue<TrophyType>();
            count = 0;
        }

        public override void Init() { }

        protected override void LoadData(XmlElement element)
        {
            type = XmlHelper.Value<TrophyType>(element, XmlConsts.Type);
            count = XmlHelper.ValueInt(element, XmlConsts.Count);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.Type, type);
            XmlHelper.AppendElement(element, XmlConsts.Count, count);
        }

        public override string ToString() => 
            $"{count} - {TypeHelper.Name(type)}";
    }
}
