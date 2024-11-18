using OxDAOEngine.Data;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class Storages : ListDAO<Storage>
    {
        public Storage? GetById(Guid id) =>
            Find(s => s.Id.Equals(id));

        public string StorageName(Guid id)
        {
            Storage? storage = GetById(id);

            return storage is null
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

        public override string ShortString
        {
            get
            {
                string result = base.ShortString;

                if (Count is 0)
                    return result;

                if (Count is 1)
                    return $"{result} ({this[0].Size} Gb)";

                int totalSize = 0;

                foreach (Storage storage in this)
                    totalSize += int.Parse(storage.Size);

                return $"{result} (total {totalSize} Gb)";
            }
        }
    }
}