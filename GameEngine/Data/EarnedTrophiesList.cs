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

        public EarnedTrophies GetTrophies(Guid accountId)
        {
            EarnedTrophies? found = Find(e => e.AccountId == accountId);
            found ??= Add(
                new() 
                { 
                    AccountId = accountId
                }
            );
            return found;
        }
    }
}
