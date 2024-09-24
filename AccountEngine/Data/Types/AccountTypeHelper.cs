using OxDAOEngine.Data.Types;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.AccountEngine.Data.Types
{
    public class AccountTypeHelper : FieldAccordingHelper<AccountField, AccountType>
    {
        public override AccountType EmptyValue() => AccountType.PSN;

        public override string GetName(AccountType value) =>
            value switch
            {
                AccountType.PSN => "PSN",
                AccountType.Local => "Local",
                _ => string.Empty
            };

        public override Color GetBaseColor(AccountType value) =>
          value switch
          {
              AccountType.PSN => Color.FromArgb(245, 251, 232),
              AccountType.Local => Color.FromArgb(255, 250, 210),
              _ => base.GetBaseColor(value)
          };
    }
}
