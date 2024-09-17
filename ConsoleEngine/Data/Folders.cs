using OxDAOEngine.Data;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class Folders : ListDAO<Folder>
    {
        public Folder? GetByName(string name) =>
            Find(f => f.Name == name);

        public string CheckName(string name)
        {
            if (name == string.Empty)
                return name;

            Folder? folder = GetByName(name);

            return folder == null
                ? "[LOST FOLDER]"
                : folder.Name;
        }
    }
}
