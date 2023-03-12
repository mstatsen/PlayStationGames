using System.Collections.Generic;

namespace PlayStationGames.GameEngine.Data.Types
{
    public enum PlayMode
    {
        SinglePlayer,
        OneScreenCooperative,
        OneScreenBattle,
        OnlineCooperative,
        OnlineMatch
    }

    public class PlayModeList : List<PlayMode>
    { 
    }
}