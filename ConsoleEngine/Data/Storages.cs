using OxXMLEngine.Data;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class Storages : ListDAO<Storage>
    {
        public Storage? GetById(Guid id) =>
            Find(s => s.Id == id);

        public string StorageName(Guid id)
        {
            Storage? storage = GetById(id);

            return storage == null
                ? "[LOST STORAGE]"
                : storage.Name;
        }

        public int GamesCount()
        {
            int result = 0;

            foreach (var storage in this)
                result += storage.GameCount;

            return result;
        }
    }
}