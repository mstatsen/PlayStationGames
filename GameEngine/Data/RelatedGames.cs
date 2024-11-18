using OxDAOEngine.Data;

namespace PlayStationGames.GameEngine.Data
{
    public class RelatedGames : ListDAO<RelatedGame>
    {
        public RelatedGame? GetById(Guid? id) =>
            List.Find(rg => rg.GameId.Equals(id));

        public bool Remove(Guid? id)
        {
            RelatedGame? relatedGame = GetById(id);
            return 
                relatedGame is not null 
                && Remove(relatedGame);
        }

        public RelatedGame? Add(Guid id)
        {
            RelatedGame? relatedGame = GetById(id);

            switch (relatedGame)
            {
                case null:
                    relatedGame = Add(new RelatedGame()
                    {
                        GameId = id
                    });
                    break;
            }

            return relatedGame;
        }
    }
}
