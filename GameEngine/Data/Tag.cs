using OxDAOEngine.Data;
using OxDAOEngine.XML;

namespace PlayStationGames.GameEngine.Data
{
    public class Tag : SimpleNameDAO
    {
        public Tag() 
        {
            XmlName = XmlConsts.Tag;
        }
    }
}