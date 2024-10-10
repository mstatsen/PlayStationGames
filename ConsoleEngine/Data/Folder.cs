using OxDAOEngine.Data;
using OxDAOEngine.XML;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class Folder : SimpleNameDAO 
    {
        public Folder() : base() => 
            XmlName = XmlConsts.Folder;
    }
}
