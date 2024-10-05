using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Decorator
{
    public class GameCalculations
    {
        public GameCalculations(Game game) =>
            Game = game;

        public int GetEarnedPoints() => 0;
        //TODO: replace with game.EarnedTrophies
        //GetFullPoints(Game.Trophyset.Earned);

        public int GetEarnedOldPoints() => 0;
            //TODO: replace with game.EarnedTrophies
            //GetFullOldPoints(Game.Trophyset.Earned);

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

        private int GetEarnedSimplePoints() => 0;
            //TODO: replace with game.EarnedTrophies
            //GetSimplePoints(Game.Trophyset.Earned);

        private int GetAvailableSimplePoints() =>
            GetSimplePoints(Game.Trophyset.Available);

        private readonly Game Game;

        private int GetAvailablePoints() =>
            GetFullPoints(Game.Trophyset.Available);
    }
}