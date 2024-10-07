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
    }
}
