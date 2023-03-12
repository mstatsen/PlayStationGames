using OxLibrary;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using OxXMLEngine.Data.Types;
using OxXMLEngine.View;

namespace PlayStationGames.ConsoleEngine.View
{
    public class ConsoleColorer : ItemColorer<ConsoleField, PSConsole>
    {
        public override Color BaseColor(PSConsole? item) => 
            new OxColorHelper(TypeHelper.BackColor(item?.Firmware)).Darker(7);
    }
}