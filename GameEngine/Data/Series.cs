using OxDAOEngine.Data;
using OxDAOEngine.XML;

namespace PlayStationGames.GameEngine.Data
{
    public class Series : SimpleNameDAO
    {
        public Series()
        {
            XmlName = XmlConsts.Series;
        }
    }
}