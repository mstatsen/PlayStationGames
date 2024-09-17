using OxDAOEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class PlayModeGroupHelper
        : AbstractTypeHelper<PlayModeGroup>
    {
        public override PlayModeGroup EmptyValue() => PlayModeGroup.SinglePlayer;

        public override string GetName(PlayModeGroup value) => 
            value switch
            {
                PlayModeGroup.OneScreenMultiplayer => "One screen",
                PlayModeGroup.OnlineMultiplayer => "Online",
                _ => "Single player",
            };

        public override string GetFullName(PlayModeGroup value) => 
            value switch
            {
                PlayModeGroup.OneScreenMultiplayer => "One screen multiplayer",
                PlayModeGroup.OnlineMultiplayer => "Online multiplayer",
                _ => "Single player",
            };

        public bool IsMultiplayer(PlayModeGroup group) =>
            group != PlayModeGroup.SinglePlayer;

        public PlayModeList Modes(PlayModeGroup group)
        {
            PlayModeList list = new();

            foreach (PlayMode mode in TypeHelper.All<PlayMode>())
                if (TypeHelper.Helper<PlayModeHelper>().Group(mode) == group)
                    list.Add(mode);

            return list;
        }
    }
}