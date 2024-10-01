using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data
{
    public class TrophyList : ListDAO<Trophy>
    {
        public Trophy Add(TrophyType type, int count) => 
            Add(new()
            {
                Type = type,
                Count = count
            });

        private Trophy? TrophyByType(TrophyType type) => 
            Find(t => t.Type == type);

        private int GetTrophyCount(TrophyType type)
        {
            Trophy? trophy = TrophyByType(type);
            return trophy == null ? 0 : trophy.Count;
        }

        private void SetTrophyCount(TrophyType type, int value)
        {
            Trophy? trophy = TrophyByType(type);

            if (trophy == null)
                Add(type, value);
            else
                trophy.Count = value;
        }

        public int Platinum
        { 
            get => GetTrophyCount(TrophyType.Platinum);
            set => SetTrophyCount(TrophyType.Platinum, value);
        }

        public int Gold
        {
            get => GetTrophyCount(TrophyType.Gold);
            set => SetTrophyCount(TrophyType.Gold, value);
        }

        public int Silver
        {
            get => GetTrophyCount(TrophyType.Silver);
            set => SetTrophyCount(TrophyType.Silver, value);
        }

        public int Bronze
        {
            get => GetTrophyCount(TrophyType.Bronze);
            set => SetTrophyCount(TrophyType.Bronze, value);
        }

        public int FromDLC
        {
            get => GetTrophyCount(TrophyType.FromDLC);
            set => SetTrophyCount(TrophyType.FromDLC, value);
        }
    }
}