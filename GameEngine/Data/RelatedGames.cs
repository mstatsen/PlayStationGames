using OxXMLEngine.Data;

namespace PlayStationGames.GameEngine.Data
{
    public class RelatedGames : ListDAO<RelatedGame>
    {
        public RelatedGame? GetById(Guid? id) =>
            List.Find((rg) => rg.GameId.Equals(id));

        public bool Remove(Guid? id)
        {
            RelatedGame? relatedGame = GetById(id);
            return relatedGame != null && Remove(relatedGame);
        }

        public RelatedGame? Add(Guid id, bool sameTrophyset)
        {
            RelatedGame? relatedGame = GetById(id);

            switch (relatedGame)
            {
                case null:
                    relatedGame = Add(new RelatedGame()
                    {
                        GameId = id,
                        SameTrophyset = sameTrophyset
                    });
                    break;
                default:
                    relatedGame.SameTrophyset = sameTrophyset;
                    break;
            }

            return relatedGame;
        }
    }
}
