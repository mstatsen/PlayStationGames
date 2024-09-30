using OxDAOEngine.ControlFactory.Initializers;
using OxDAOEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data;
using OxDAOEngine.Data.Filter;

namespace PlayStationGames.ConsoleEngine.ControlFactory.Initializers
{
    public class ConsoleInitializer : ExtractInitializer<ConsoleField, PSConsole>,
        IComboBoxInitializer
    {
        public ListDAO<PSConsole>? ExistingConsoles;

        public ConsoleInitializer(ConsoleField field, bool addAnyObject = false, bool fullExtract = false, bool fixedExtract = false, IMatcher<ConsoleField>? filter = null) 
            : base(field, addAnyObject, fullExtract, fixedExtract, filter)
        {
        }

        public bool AvailableValue(object value) =>
            ExistingConsoles == null
            || (value is PSConsole console && !ExistingConsoles.Contains(l => l.Id == console.Id));
    }
}