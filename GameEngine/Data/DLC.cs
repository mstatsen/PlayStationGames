using System.Xml;
using OxDAOEngine.Data;
using OxDAOEngine.XML;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data
{
    public class DLC : RootDAO<GameField>
    {
        public DLC() : base() { }
            
        private bool acquired = false;
        public readonly Trophyset Trophyset = new();

        public bool Acquired
        {
            get => acquired;
            set => acquired = BoolValue(ModifyValue(acquired, value));
        }

        public override void Clear()
        {
            base.Clear();
            acquired = false;
            Trophyset.Clear();
        }

        public override void Init()
        {
            base.Init();
            AddMember(Trophyset);
        }

        protected override void LoadData(XmlElement element)
        {
            base.LoadData(element);
            acquired = XmlHelper.ValueBool(element, XmlConsts.Aqcuired);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            base.SaveData(element, clearModified);
            XmlHelper.AppendElement(element, XmlConsts.Aqcuired, Acquired);
        }

        public override string ToString() => 
            Name;

        public override bool Equals(object? obj) =>
            base.Equals(obj)
            && obj is DLC otherDLC
            && Acquired.Equals(otherDLC.Acquired)
            && Trophyset.Equals(otherDLC.Trophyset);

        public override int GetHashCode() => 
            539060726 + EqualityComparer<string>.Default.GetHashCode(Name);

        protected override bool AlwaysSaveImage => true;
    }
}