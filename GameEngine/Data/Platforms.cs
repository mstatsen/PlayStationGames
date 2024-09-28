using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data
{
    public class Platforms : ListDAO<Platform>
    {
        public void Add(PlatformType type)
        {
            if (!Contains(type))
                Add(new Platform(type));
        }

        public bool Contains(PlatformType platformType) =>
            Contains(p => p.Type.Equals(platformType));

        public Platforms() : base() { }

        public Platforms(PlatformType platformType) : this() =>
            Add(platformType);
    }
}