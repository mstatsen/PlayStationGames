using OxDAOEngine.Data;
using OxDAOEngine.Data.Links;
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

        public readonly TrophyList Earned = new()
        {
            XmlName = "Earned",
            SaveEmptyList = false
        };

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
            Earned.Clear();
        }

        public override void Init() 
        {
            AddMember(Available);
            AddMember(Earned);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.Type, Type);
            XmlHelper.AppendElement(element, XmlConsts.Difficult, Difficult);
            XmlHelper.AppendElement(element, XmlConsts.CompleteTime, CompleteTime);
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
            && Earned.Equals(otherTrophyset.Earned)
            && Available.Equals(otherTrophyset.Available)
            && Type.Equals(otherTrophyset.Type);

        public override int GetHashCode() => 
            2049151605 + Type.GetHashCode();

        public override string ToString() =>
            TypeHelper.ShortName(Type);

        public Trophyset() : base() { }

        public override int CompareTo(DAO? other)
        {
            if (Equals(other))
                return 0;

            if (other == null)
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

            return Earned.CompareTo(otherTrophyset.Earned); ;
        }
    }
}
