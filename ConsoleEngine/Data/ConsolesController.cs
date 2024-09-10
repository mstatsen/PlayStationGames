using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Decorator;
using OxXMLEngine.Data.Fields;
using OxXMLEngine.Data.Sorting;
using OxXMLEngine.Data.Types;
using OxXMLEngine.Editor;
using PlayStationGames.ConsoleEngine.ControlFactory;
using PlayStationGames.ConsoleEngine.Data.Decorator;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Editor;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class ConsolesController : ListController<
        ConsoleField, 
        PSConsole, 
        ConsoleFieldGroup, 
        ConsolesController>
    {
        public override string Name => "Consoles";

        protected override DecoratorFactory<ConsoleField, PSConsole> CreateDecoratorFactory() =>
            new ConsoleDecoratorFactory();

        protected override DAOEditor<ConsoleField, PSConsole, ConsoleFieldGroup> CreateEditor() =>
            new ConsoleEditor();

        protected override DAOWorker<ConsoleField, PSConsole, ConsoleFieldGroup> CreateWorker() =>
            new ConsoleWorker();

        protected override FieldHelper<ConsoleField> RegisterFieldHelper() =>
            TypeHelper.Register<ConsoleFieldHelper>();

        protected override FieldGroupHelper<ConsoleField, ConsoleFieldGroup> RegisterFieldGroupHelper() =>
            TypeHelper.Register<ConsoleFieldGroupHelper>();

        protected override void RegisterHelpers()
        {
            TypeHelper.Register<ConsoleFieldGroupHelper>();
            TypeHelper.Register<ConsoleFieldHelper>();
            TypeHelper.Register<ConsoleGenerationHelper>();
            TypeHelper.Register<ConsoleModelHelper>();
            TypeHelper.Register<StoragePlacementHelper>();
            TypeHelper.Register<FirmwareTypeHelper>();
            TypeHelper.Register<AccessoryTypeHelper>();
        }

        protected override ControlFactory<ConsoleField, PSConsole> CreateControlFactory() =>
            new ConsoleControlFactory();

        public override FieldSortings<ConsoleField, PSConsole> DefaultSorting() => 
            new()
            {
                new FieldSorting<ConsoleField, PSConsole>(ConsoleField.Name, SortOrder.Ascending)
            };
    }
}
