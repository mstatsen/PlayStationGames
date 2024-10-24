using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data
{
    public class TrophyList : ListDAO<Trophy>
    {
        public Trophy Add(TrophyType type, int count)
        {
            Trophy? existTrophy = 
                Find(t => t.Type == type) 
                ?? Add(new Trophy()
                {
                    Type = type,
                    Count = 0
                });

            existTrophy.Count += count;
            return existTrophy;
        }

        public void Add(ListDAO<Trophy>? trophyList)
        {
            if (trophyList == null)
                return;

            foreach (Trophy trophy in trophyList)
                Add(trophy.Type, trophy.Count);
        }

        public Trophy? TrophyByType(TrophyType type) => 
            Find(t => t.Type == type);

        public int GetTrophyCount(TrophyType type)
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

        public int TotalTrophiesCount =>
            Platinum + Gold + Silver + Bronze;

        public int TrophiesCount(TrophyType type) =>
            type switch
            {
                TrophyType.Platinum => Platinum,
                TrophyType.Gold => Gold,
                TrophyType.Silver => Silver,
                TrophyType.Bronze => Bronze,
                _ => 0,
            };

        private readonly TrophyTypeHelper TrophyTypeHelper = TypeHelper.Helper<TrophyTypeHelper>();
        public int Points
        {
            get
            {
                int points = 0;

                foreach (Trophy trophy in this)
                    points += TrophyTypeHelper.Points(trophy.Type) * trophy.Count;

                return points;
            }
        }

        public int OldPoints
        {
            get
            {
                int points = 0;

                foreach (Trophy trophy in this)
                    points += TrophyTypeHelper.OldPoints(trophy.Type) * trophy.Count;

                return points;
            }
        }

    }
}