using OxDAOEngine.Data;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class Folders : ListDAO<Folder>
    {
        public Folder? GetByName(string name) =>
            Find(f => f.Name.Equals(name));

        public string CheckName(string name)
        {
            if (name.Equals(string.Empty))
                return name;

            Folder? folder = GetByName(name);

            return folder is null
                ? "[LOST FOLDER]"
                : folder.Name;
        }
    }
}
