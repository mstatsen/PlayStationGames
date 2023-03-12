using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    public class GameCalculations
    {
        public GameCalculations(Game game) =>
            Game = game;

        public int GetEarnedPoints() =>
            GetFullPoints(Game.EarnedTrophies);

        public int GetEarnedOldPoints() =>
            GetFullOldPoints(Game.EarnedTrophies);

        public Status GetGameStatus()
        {
            if (!AvailableTrophiesExist())
                return Status.Unknown;
            else
            {
                int earnedPoints = GetEarnedPoints();
                return earnedPoints == 0
                    ? Status.NotStarted
                    : earnedPoints == GetAvailablePoints()
                        ? Status.Completed
                        : Status.Started;
            }
        }

        public int GetGameProgress()
        {
            int availablePoints = GetAvailableSimplePoints();
            int earnedPoints = GetEarnedSimplePoints();

            if (availablePoints == 0)
                return 0;
            else
            {
                int progress = (earnedPoints * 100) / (availablePoints);
                return (progress == 0) && (earnedPoints > 0) ? 1 : progress;
            }
        }

        public bool AvailableTrophiesExist() =>
            GetAvailablePoints() > 0;

        private static int GetSimplePoints(TrophyList trophyset) =>
            CalcPoints(trophyset, 0);

        private static int CalcPoints(TrophyList trophyset, int platinumMultiplier) =>
            trophyset.Platinum * platinumMultiplier +
                trophyset.Gold * 90 +
                trophyset.Silver * 30 +
                trophyset.Bronze * 15;

        private static int GetFullPoints(TrophyList trophyset) =>
            CalcPoints(trophyset, 300);

        private static int GetFullOldPoints(TrophyList trophyset) =>
            CalcPoints(trophyset, 180);

        private int GetEarnedSimplePoints() =>
            GetSimplePoints(Game.EarnedTrophies);

        private int GetAvailableSimplePoints() =>
            GetSimplePoints(Game.AvailableTrophies);

        private readonly Game Game;

        private int GetAvailablePoints() =>
            GetFullPoints(Game.AvailableTrophies);
    }
}