using OxDAOEngine.Data.Types;
using PlayStationGames.AccountEngine.Data.Fields;

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
    }
}
