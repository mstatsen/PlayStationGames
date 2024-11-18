using OxDAOEngine.Data;

namespace PlayStationGames.GameEngine.Data
{
    public class EarnedTrophiesList : ListDAO<EarnedTrophies>
    {
        public EarnedTrophiesList() 
        {
            XmlName = "Earned";
            SaveEmptyList = false;
        }

        public EarnedTrophies Add(Guid accountId, TrophyList trophyList)
        {
            EarnedTrophies? existsEarned = Find(e => e.AccountId.Equals(accountId));

            if (existsEarned is null)
            {
                existsEarned = Add();
                existsEarned.AccountId = accountId;
            }
            
            existsEarned.Trophies.Add(trophyList);
            return existsEarned;
        }

        public TrophyList? GetTrophies(Guid? accountId) =>
            Find(e => e.AccountId.Equals(accountId))?.Trophies;
    }
}
