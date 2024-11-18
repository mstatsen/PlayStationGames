using OxDAOEngine.Data;
using OxDAOEngine.XML;
using PlayStationGames.GameEngine.Data.Fields;
using System.Xml;

namespace PlayStationGames.GameEngine.Data
{
    public class RelatedGame : DAO
    {
        public RelatedGame() => WithoutXmlNode = true;
        
        private Guid gameId;

        private readonly RootListDAO<GameField, Game> FullGamesList = 
            DataManager.ListController<GameField, Game>().FullItemsList;

        public Game? Game => FullGamesList.Find(g => g.Id == gameId);

        public override void Clear() => 
            GameId = Guid.Empty;

        public override void Init() { }

        public Guid GameId
        {
            get => gameId;
            set => gameId = ModifyValue(gameId, value);
        }

        protected override void LoadData(XmlElement element) => 
            gameId = GuidValue(element.InnerText);

        protected override void SaveData(XmlElement element, bool clearModified = true) =>
            XmlHelper.AppendElement(element, XmlConsts.RelatedGame, GameId);

        public override string ToString()
        {
            Game? game = DataManager.Item<GameField, Game>(GameField.Id, GameId);
            return game is null 
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