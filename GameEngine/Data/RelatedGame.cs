using OxDAOEngine.Data;
using OxDAOEngine.XML;
using PlayStationGames.GameEngine.Data.Fields;
using System.Xml;

namespace PlayStationGames.GameEngine.Data
{
    public class RelatedGame : DAO
    {
        private Guid gameId;
        private bool sameTrophyset;

        public override void Clear()
        {
            GameId = Guid.Empty;
            SameTrophyset = false;
        }

        public override void Init() { }

        public Guid GameId
        {
            get => gameId;
            set => gameId = ModifyValue(gameId, value);
        }

        public bool SameTrophyset
        {
            get => sameTrophyset;
            set => sameTrophyset = ModifyValue(sameTrophyset, value);
        }

        protected override void LoadData(XmlElement element)
        {
            gameId = XmlHelper.ValueGuid(element, XmlConsts.GameId);
            sameTrophyset = XmlHelper.ValueBool(element, XmlConsts.SameTrophyset);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.GameId, GameId);

            if (sameTrophyset)
                XmlHelper.AppendElement(element, XmlConsts.SameTrophyset, SameTrophyset);
        }

        public override string ToString()
        {
            Game? game = DataManager.Item<GameField, Game>(GameField.Id, GameId);

            string? gameName =
                game == null
                    ? GameId.ToString() 
                    : game.FullTitle();

            string sameTrophysetString = 
                SameTrophyset 
                    ? " [same trophies]"
                    : string.Empty ;

            return $"{gameName}{sameTrophysetString}";
        }

        public override bool Equals(object? obj) => 
            obj is RelatedGame otherRelatedGame
            && (base.Equals(obj)
                || (GameId.Equals(otherRelatedGame.GameId)
                    && SameTrophyset.Equals(otherRelatedGame.SameTrophyset)));

        public override int GetHashCode() => 
            HashCode.Combine(GameId, SameTrophyset);
    }
}
