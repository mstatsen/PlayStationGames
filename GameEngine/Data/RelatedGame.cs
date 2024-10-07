using OxDAOEngine.Data;
using OxDAOEngine.XML;
using PlayStationGames.GameEngine.Data.Fields;
using System.Xml;

namespace PlayStationGames.GameEngine.Data
{
    public class RelatedGame : DAO
    {
        private Guid gameId;

        public override void Clear() => 
            GameId = Guid.Empty;

        public override void Init() { }

        public Guid GameId
        {
            get => gameId;
            set => gameId = ModifyValue(gameId, value);
        }


        protected override void LoadData(XmlElement element) => 
            gameId = XmlHelper.ValueGuid(element, XmlConsts.GameId);

        protected override void SaveData(XmlElement element, bool clearModified = true) => 
            XmlHelper.AppendElement(element, XmlConsts.GameId, GameId);

        public override string ToString()
        {
            Game? game = DataManager.Item<GameField, Game>(GameField.Id, GameId);
            return game == null 
                ? GameId.ToString() 
                : game.FullTitle();
        }

        public override bool Equals(object? obj) => 
            obj is RelatedGame otherRelatedGame
            && (base.Equals(obj)
                || GameId.Equals(otherRelatedGame.GameId));

        public override int GetHashCode() => 
            GameId.GetHashCode();
    }
}
