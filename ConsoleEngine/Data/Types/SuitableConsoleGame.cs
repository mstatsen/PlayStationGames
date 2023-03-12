using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.ConsoleEngine.Data.Types
{
    public class SuitableConsoleGame
    {
        public PlatformType PlatformType { get; private set; }
        public Source Source { get; private set; }
        public bool Licensed { get; private set; }

        public SuitableConsoleGame(PlatformType platformType,
            Source source, bool licensed)
        {
            PlatformType = platformType;
            Source = source;
            Licensed = licensed;
        }
    }
}