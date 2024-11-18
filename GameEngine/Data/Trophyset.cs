using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxDAOEngine.XML;
using PlayStationGames.GameEngine.Data.Types;
using System.Xml;

namespace PlayStationGames.GameEngine.Data
{
    public class Trophyset : DAO
    {
        private TrophysetType type;
        private Difficult difficult = TypeHelper.DefaultValue<Difficult>();

        private CompleteTime completeTime = TypeHelper.DefaultValue<CompleteTime>();
        public Difficult Difficult
        {
            get => difficult;
            set => difficult = ModifyValue(difficult, value);
        }

        public CompleteTime CompleteTime
        {
            get => completeTime;
            set => completeTime = ModifyValue(completeTime, value);
        }

        public TrophysetType Type
        {
            get => type;
            set => type = ModifyValue(type, value);
        }

        public readonly Platforms AppliesTo = new()
        {
            XmlName = "AppliesTo",
            SaveEmptyList = false
        };

        public readonly EarnedTrophiesList EarnedTrophies = new();

        public TrophyList Available = new()
        {
            XmlName = "Available",
            SaveEmptyList = false
        };

        public override void Clear()
        {
            type = TypeHelper.DefaultValue<TrophysetType>();
            difficult = TypeHelper.DefaultValue<Difficult>();
            completeTime = TypeHelper.DefaultValue<CompleteTime>();
            Available.Clear();
            EarnedTrophies.Clear();
            AppliesTo.Clear();
        }

        public override void Init() 
        {
            AddMember(Available);
            AddMember(EarnedTrophies);
            AddMember(AppliesTo);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.Type, Type);

            if (TrophysetExists)
            {
                XmlHelper.AppendElement(element, XmlConsts.Difficult, Difficult);
                XmlHelper.AppendElement(element, XmlConsts.CompleteTime, CompleteTime);
            }
       }

        protected override void LoadData(XmlElement element)
        {
            type = XmlHelper.Value<TrophysetType>(element, XmlConsts.Type);
            difficult = XmlHelper.Value<Difficult>(element, XmlConsts.Difficult);
            completeTime = XmlHelper.Value<CompleteTime>(element, XmlConsts.CompleteTime);
        }

        public override bool Equals(object? obj) => 
            obj is Trophyset otherTrophyset
            && Difficult.Equals(otherTrophyset.Difficult)
            && CompleteTime.Equals(otherTrophyset.CompleteTime)
            && EarnedTrophies.Equals(otherTrophyset.EarnedTrophies)
            && Available.Equals(otherTrophyset.Available)
            && Type.Equals(otherTrophyset.Type)
            && AppliesTo.Equals(otherTrophyset.AppliesTo);

        public override int GetHashCode() => 
            2049151605 + Type.GetHashCode();

        public override string ToString() =>
            TypeHelper.ShortName(Type);

        public Trophyset() : base() { }

        public override int CompareTo(IDAO? other)
        {
            if (Equals(other))
                return 0;

            if (other is null)
                return 1;

            Trophyset otherTrophyset = (Trophyset)other;
            int result = Type.CompareTo(otherTrophyset.Type);

            if (result != 0) 
                return result;

            result = Difficult.CompareTo(otherTrophyset.Difficult);

            if (result != 0)
                return result;

            result = CompleteTime.CompareTo(otherTrophyset.CompleteTime);

            if (result != 0)
                return result;

            result = Available.CompareTo(otherTrophyset.Available);

            if (result != 0)
                return result;

            result = EarnedTrophies.CompareTo(otherTrophyset.EarnedTrophies);

            if (result != 0)
                return result;

            return AppliesTo.CompareTo(otherTrophyset.AppliesTo);
        }

        public void AddTrophies(Trophyset trophyset)
        {
            Available.Add(trophyset.Available);

            foreach (EarnedTrophies otherEarned in trophyset.EarnedTrophies)
                EarnedTrophies.Add(otherEarned.AccountId, otherEarned.Trophies);
        }

        public TrophyList? GetTrophies(Guid? accountId) => 
            accountId is not null
                ? EarnedTrophies.GetTrophies(accountId) 
                : Available;

        public bool ExistsTrophies(Guid accountId) =>
            EarnedTrophies.Contains(e => e.AccountId.Equals(accountId));

        public bool TrophysetIsComplete(Guid accountId)
        {
            TrophyList? trophyList = GetTrophies(accountId);
            return 
                trophyList is not null
                && trophyList.Points.Equals(Available.Points);
        }

        public bool TrophysetExists => 
            Type != TrophysetType.NoSet;
    }

    public class FullTrophyset : Trophyset
    { 
        public string GameName { get; private set; }

        public FullTrophyset(string gameName) => 
            GameName = gameName;

        public override string DefaultXmlElementName => "Trophyset";

        public override bool Equals(object? obj) =>
            base.Equals(obj)
            && obj is FullTrophyset otherFullTrophyset
            && GameName.Equals(otherFullTrophyset.GameName);

        public override int GetHashCode() => 
            base.GetHashCode() ^ GameName.GetHashCode();
    }
}