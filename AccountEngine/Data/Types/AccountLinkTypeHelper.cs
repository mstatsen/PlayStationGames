using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.PSNEngine.Data.Types;

namespace PlayStationGames.AccountEngine.Data.Types
{
    public class AccountLinkTypeHelper : PSNLinkTypeHelper<AccountField>
    {
        protected override AccountField GetExtractFieldName() => AccountField.Links;
    }
}
