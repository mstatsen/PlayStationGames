using OxDAOEngine.ControlFactory.Initializers;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Types;

namespace PlayStationGames.GameEngine.ControlFactory.Initializers
{
    public class OwnerInitializer : ComboBoxInitializer
    {
        public override bool AvailableValue(object value)
        {
            if (value is Account account)
                return account.Type == AccountType.PSN;

            return false;
        }
    }
}