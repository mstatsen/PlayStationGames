using OxDAOEngine.Data;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data
{
    public class UniqueTrophysets : Dictionary<Trophyset, ListDAO<Game>>
    {
        private readonly Dictionary<Guid, ListDAO<Trophyset>> AccountsTrophysets = new();
        private readonly Dictionary<Guid, Trophyset> AccountsFullTrophysets = new();

        public ListDAO<Trophyset> Trophysets(Guid accountID)
        {
            if (!AccountsTrophysets.TryGetValue(accountID, out ListDAO<Trophyset>? trophysets))
            {
                trophysets = new();
                AccountsTrophysets.Add(accountID, trophysets);

                foreach (Trophyset trophiset in Keys)
                    if (trophiset.ExistsTrophies(accountID))
                        trophysets.Add(trophiset);
            }

            return trophysets;
        }

        public int TrophysetsCount(Guid accountId) => Trophysets(accountId).Count;

        public Trophyset FullTrophyset(Guid accountID)
        {
            if (!AccountsFullTrophysets.TryGetValue(accountID, out Trophyset? fullTrophyset))
            {
                fullTrophyset = new();
                AccountsFullTrophysets.Add(accountID, fullTrophyset);

                foreach (Trophyset trophiset in Trophysets(accountID))
                    fullTrophyset.AddTrophies(trophiset);
            }

            return fullTrophyset;
        }

        public TrophyList? TrophyList(Guid accountId) =>
            FullTrophyset(accountId).GetTrophies(accountId);

        public int TrophiesCount(Guid accountId)
        {
            TrophyList? trophyList = TrophyList(accountId);
            return trophyList == null ? 0 : trophyList.TotalTrophiesCount;
        }

        public int TrophiesCount(Guid accountId, TrophyType type)
        {
            TrophyList? trophyList = TrophyList(accountId);
            return trophyList == null ? 0 : trophyList.TrophiesCount(type);
        }

        public int CompletedCount(Guid accountId) =>
            Trophysets(accountId).Count(t => t.TrophysetIsComplete(accountId));

        public int Points(Guid accountId)
        {
            TrophyList? trophyList = TrophyList(accountId);
            return trophyList == null ? 0 : trophyList.Points;
        }

        public int OldPoints(Guid accountId)
        {
            TrophyList? trophyList = TrophyList(accountId);
            return trophyList == null ? 0 : trophyList.OldPoints;
        }

        public string PSNLevel(Guid accountId) =>
            PSNLevel(PSNLevelInfo.Levels, accountId);

        public string OldPSNLevel(Guid accountId) =>
            PSNLevel(PSNLevelInfo.OldLevels, accountId);

        private string PSNLevel(List<PSNLevelInfo> levels, Guid accountId)
        {
            int intLevel = 0;
            PSNLevelInfo lastLevelInfo = levels[0];
            int earnedPoints = Points(accountId) + lastLevelInfo.LevelUpPoint;

            foreach (PSNLevelInfo levelInfo in levels)
                while (earnedPoints > levelInfo.LevelUpPoint)
                {
                    lastLevelInfo = levelInfo;
                    earnedPoints -= levelInfo.LevelUpPoint;
                    intLevel++;

                    if (intLevel > levelInfo.Level)
                        break;
                }

            string level = $"{intLevel}";

            if (intLevel > 0)
            {
                float percent = 0;

                if (earnedPoints > 0)
                    percent = earnedPoints * 100 / (float)lastLevelInfo.LevelUpPoint;

                level += $" ({(int)percent}%)";
            }

            return level;
        }

        public new void Clear()
        {
            base.Clear();
            AccountsTrophysets.Clear();
            AccountsFullTrophysets.Clear();
        }
    }
}
