using OxLibrary;
using OxXMLEngine.View;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;

namespace PlayStationGames.AccountEngine.View
{
    public class AccountColorer : ItemColorer<AccountField, Account>
    {
        public override Color BaseColor(Account? item) => new OxColorHelper(Color.FromArgb(245, 251, 232)).Darker(7);
    }
}