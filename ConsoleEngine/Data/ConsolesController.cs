using Microsoft.VisualBasic.FileIO;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Decorator;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Data.Sorting;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Editor;
using OxLibrary;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.ControlFactory;
using PlayStationGames.ConsoleEngine.Data.Decorator;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.ConsoleEngine.Editor;
using PlayStationGames.GameEngine.Data;
using PlayStationGames.GameEngine.Data.Fields;

namespace PlayStationGames.ConsoleEngine.Data
{
    public class ConsolesController : ListController<
        ConsoleField, 
        PSConsole, 
        ConsoleFieldGroup, 
        ConsolesController>
    {
        public override string ListName => "Consoles";
        public override string ItemName => "Console";

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
            TypeHelper.Register<JoystickTypeHelper>();
        }

        protected override ControlFactory<ConsoleField, PSConsole> CreateControlFactory() =>
            new ConsoleControlFactory();

        public override FieldSortings<ConsoleField, PSConsole> DefaultSorting() => 
            new()
            {
                new FieldSorting<ConsoleField, PSConsole>(ConsoleField.Name, SortOrder.Ascending)
            };

        public override bool AvailableBatchUpdate => false;
        public override bool AvailableSummary => false;
        public override bool AvailableCategories => false;
        public override bool AvailableQuickFilter => false;

        protected override void SetHandlers() => 
            DataManager.ListController<AccountField, Account>().RemoveHandler += OnAccountRemove;

        private void OnAccountRemove(Account dao, DAOEntityEventArgs e)
        {
            foreach (PSConsole console in FullItemsList.FindAll((c) => c.Accounts.Contains((ca) => ca.Id == dao.Id)))
                console.Accounts.RemoveAll((c) => c.Id == dao.Id);
        }

        protected override Bitmap? GetIcon() => OxIcons.Console;

        public override List<ToolStripMenuItem>? MenuItems(PSConsole? item)
        {
            if (item == null || 
                !TypeHelper.Helper<ConsoleGenerationHelper>().StorageSupport(item.Generation))
                return null;
            List<ToolStripMenuItem> result = new();

            ToolStripMenuItem showGamesItem = new(
                "Show installed games", 
                OxIcons.Install,
                (s, e) => DataManager.ViewItems<GameField, Game>(
                    g => g.Installations.Contains(i => i.ConsoleId == item.Id), $"Games, installed on {item}")
            );
            result.Add(showGamesItem);
            return result;
        }
    }
}
