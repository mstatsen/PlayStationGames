using System.Collections.Generic;

namespace PlayStationGames.GameEngine.Summary
{
    internal class LevelInfo
    {
        public int LevelUpPoint { get; internal set; }
        public int Level { get; internal set; }
        public LevelInfo(int level, int levelUpPoint)
        {
            Level = level;
            LevelUpPoint = levelUpPoint;
        }

        public static readonly List<LevelInfo> NewLevels = 
            new()
            {
                new LevelInfo(99, 60),
                new LevelInfo(199, 90),
                new LevelInfo(299, 450),
                new LevelInfo(399, 900),
                new LevelInfo(499, 1350),
                new LevelInfo(599, 1800),
                new LevelInfo(699, 2250),
                new LevelInfo(799, 2700),
                new LevelInfo(899, 3150),
                new LevelInfo(9999, 3600)
            };

        public static readonly List<LevelInfo> OldLevels = 
            new()
            {
                new LevelInfo(1, 200),
                new LevelInfo(2, 400),
                new LevelInfo(3, 600),
                new LevelInfo(4, 1200),
                new LevelInfo(5, 1600),
                new LevelInfo(11, 2000),
                new LevelInfo(25, 8000),
                new LevelInfo(999, 10000),
            };
    }
}