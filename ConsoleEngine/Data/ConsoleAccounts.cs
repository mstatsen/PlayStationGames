using OxXMLEngine.Data;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class ConsoleAccounts : ListDAO<ConsoleAccount>
    {
        public ConsoleAccount? GetById(Guid id) =>
            Find(s => s.Id == id);

        public string AccountName(Guid id)
        {
            ConsoleAccount? account = GetById(id);

            return account == null
                ? "[LOST ACCOUNT]"
                : account.ToString();
        }
    }
}