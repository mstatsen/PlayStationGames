using OxLibrary;
using OxDAOEngine.Data.Types;
using OxDAOEngine.View;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.View
{
    public class ConsoleColorer : ItemColorer<ConsoleField, PSConsole>
    {
        public override Color BaseColor(PSConsole? item) => 
            new OxColorHelper(TypeHelper.BackColor(item?.Firmware)).Darker(7);
    }
}