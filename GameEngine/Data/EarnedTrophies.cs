using OxDAOEngine.Data;
using OxDAOEngine.XML;
using System.Xml;

namespace PlayStationGames.GameEngine.Data
{
    public class EarnedTrophies : DAO
    {
        private Guid accountId = Guid.Empty;

        public Guid AccountId
        {
            get => accountId;
            set => accountId = ModifyValue(accountId, value);
        }

        public readonly TrophyList Trophies = new();

        public EarnedTrophies() : base() { }

        public override void Clear()
        {
            accountId = Guid.Empty;
            Trophies.Clear();
        }

        public override void Init() => 
            AddMember(Trophies);

        protected override void LoadData(XmlElement element) => 
            accountId = XmlHelper.ValueGuid(element, XmlConsts.AccountId);

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            if (Trophies.Count != 0)
                XmlHelper.AppendElement(element, XmlConsts.AccountId, accountId);
        }
    }
}