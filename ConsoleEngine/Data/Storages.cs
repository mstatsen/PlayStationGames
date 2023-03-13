using OxXMLEngine.Data;
using System;

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
    }
}