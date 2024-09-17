using OxDAOEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data.Types
{
    public class PlayModeHelper
        : AbstractTypeHelper<PlayMode>
    {
        public override PlayMode EmptyValue() => PlayMode.SinglePlayer;

        public override string GetName(PlayMode value) => 
            value switch
            {
                PlayMode.OneScreenBattle => "Coach battle",
                PlayMode.OneScreenCooperative => "One screen cooperative",
                PlayMode.OnlineMatch => "Online match",
                PlayMode.OnlineCooperative => "Online cooperative",
                _ => "Single player",
            };

        public override string GetShortName(PlayMode value) => 
            value switch
            {
                PlayMode.OneScreenBattle => "Battle",
                PlayMode.OneScreenCooperative => "Cooperative",
                PlayMode.OnlineMatch => "Match",
                PlayMode.OnlineCooperative => "Cooperative",
                _ => "Single player",
            };

        public override string GetXmlValue(PlayMode value) => 
            value switch
            {
                PlayMode.OneScreenBattle => "CoachBattle",
                PlayMode.OneScreenCooperative => "OneScreenCoopirative",
                PlayMode.OnlineMatch => "OnlineMatch",
                PlayMode.OnlineCooperative => "OnlineCooperative",
                _ => "SinglePlayer",
            };

        public PlayModeGroup Group(PlayMode mode) => 
            mode switch
            {
                PlayMode.OneScreenCooperative or 
                PlayMode.OneScreenBattle => 
                    PlayModeGroup.OneScreenMultiplayer,
                PlayMode.OnlineCooperative or 
                PlayMode.OnlineMatch => 
                    PlayModeGroup.OnlineMultiplayer,
                _ => 
                    PlayModeGroup.SinglePlayer,
            };
    }
}