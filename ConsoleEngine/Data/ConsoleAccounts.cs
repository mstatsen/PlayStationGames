using OxDAOEngine.Data;
using PlayStationGames.AccountEngine.Data;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class ConsoleAccounts : ListDAO<ConsoleAccount>
    {
        public ConsoleAccount? GetById(Guid id) =>
            Find(s => s.Id == id);

        public string? AccountName(Guid id)
        {
            ConsoleAccount? account = GetById(id);

            return account is null
                ? "[LOST ACCOUNT]"
                : account.ToString();
        }

        public void Add(Account account) =>
            Add(
                new ConsoleAccount()
                { 
                    Id = account.Id,
                }
            );
    }
}