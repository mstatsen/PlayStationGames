using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxDAOEngine.XML;
using PlayStationGames.GameEngine.Data.Types;
using System.Xml;

namespace PlayStationGames.GameEngine.Data
{
    public class GameMode : DAO
    {
        public GameMode() : base() =>
            WithoutXmlNode = true;

        public GameMode(PlayMode mode) : this() =>
            playMode = mode;

        private PlayMode playMode;

        public PlayMode PlayMode
        {
            get => playMode;
            set => playMode = ModifyValue(playMode, value);
        }

        public override void Clear() =>
            playMode = PlayMode.SinglePlayer;

        public override void Init() { }

        protected override void LoadData(XmlElement element) =>
            playMode = TypeHelper.Parse<PlayMode>(element.InnerText);

        public override bool Equals(object? obj) =>
            obj is GameMode mode && (base.Equals(obj) || mode.PlayMode == PlayMode);

        protected override void SaveData(XmlElement element, bool clearModified = true) =>
            XmlHelper.AppendElement(element, XmlConsts.Mode, PlayMode);

        public override string? ToString() => 
            $"{TypeHelper.Name(PlayMode)}";

        public override int GetHashCode() => 
            HashCode.Combine(PlayMode);

        public override int CompareTo(DAO? other)
        {
            if (other is GameMode otherMode)
            {
                int result = PlayMode.CompareTo(otherMode.PlayMode);

                if (result != 0)
                    return result;
            }

            return base.CompareTo(other);
        }
    }
}