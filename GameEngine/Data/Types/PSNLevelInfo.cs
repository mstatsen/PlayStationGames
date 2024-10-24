namespace PlayStationGames.GameEngine.Data
{
    public class PSNLevelInfo
    {
        public int LevelUpPoint { get; internal set; }
        public int Level { get; internal set; }
        private PSNLevelInfo(int level, int levelUpPoint)
        {
            Level = level;
            LevelUpPoint = levelUpPoint;
        }

        public static readonly List<PSNLevelInfo> Levels = 
            new()
            {
                new(99, 60),
                new(199, 90),
                new(299, 450),
                new(399, 900),
                new(499, 1350),
                new(599, 1800),
                new(699, 2250),
                new(799, 2700),
                new(899, 3150),
                new(9999, 3600)
            };

        public static readonly List<PSNLevelInfo> OldLevels = 
            new()
            {
                new(1, 200),
                new(2, 400),
                new(3, 600),
                new(4, 1200),
                new(5, 1600),
                new(11, 2000),
                new(25, 8000),
                new(999, 10000)
            };
    }
}