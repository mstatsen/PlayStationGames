using OxXMLEngine.Data;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class Accessories : ListDAO<Accessory>
    {
        public Accessory? GetById(Guid id) =>
            Find(s => s.Id == id);
    }
}