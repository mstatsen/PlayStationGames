using OxDAOEngine.Data;
using OxDAOEngine.Data.Types;
using OxDAOEngine.Data.Fields;
using OxDAOEngine.Data.Decorator;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Editor;
using OxDAOEngine.Settings;
using OxDAOEngine.Summary;
using OxDAOEngine.Data.Sorting;
using PlayStationGames.GameEngine.ControlFactory;
using PlayStationGames.GameEngine.Data.Decorator;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Filter;
using PlayStationGames.GameEngine.Data.Types;
using PlayStationGames.GameEngine.Editor;
using PlayStationGames.GameEngine.Summary;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using OxLibrary;
using OxDAOEngine.Data.Links;
using OxDAOEngine.Grid;

namespace PlayStationGames.GameEngine.Data
{
    public class GamesController
        : ListController<GameField, Game, GameFieldGroup, GamesController>
    {
        public GamesController() : base() { }

        protected override DAOEditor<GameField, Game, GameFieldGroup> CreateEditor() => new GameEditor();
        protected override DAOWorker<GameField, Game, GameFieldGroup> CreateWorker() => new GameWorker();

        protected override void RegisterHelpers()
        {
            TypeHelper.Register<CompleteTimeHelper>();
            TypeHelper.Register<DefaulterScopeHelper>();
            TypeHelper.Register<DifficultHelper>();
            TypeHelper.Register<GameFieldHelper>();
            TypeHelper.Register<GameFieldGroupHelper>();
            TypeHelper.Register<GameFormatHelper>();
            TypeHelper.Register<LevelValueTypeHelper>();
            TypeHelper.Register<LevelValueTypeGroupHelper>();
            TypeHelper.Register<PegiHelper>();
            TypeHelper.Register<PlatformFamilyHelper>();
            TypeHelper.Register<PlatformTypeHelper>();
            TypeHelper.Register<ScreenViewHelper>();
            TypeHelper.Register<SourceHelper>();
            TypeHelper.Register<TrophysetTypeHelper>();
            TypeHelper.Register<TrophyTypeHelper>();
            TypeHelper.Register<GameRegionHelper>();
            TypeHelper.Register<GameLanguageHelper>();
            TypeHelper.Register<GameLinkTypeHelper>();
            TypeHelper.Register<DeviceTypeHelper>();
        }

        public override string ListName => "Games";
        public override string ItemName => "Game";

        protected override FieldHelper<GameField> RegisterFieldHelper() =>
            TypeHelper.Register<GameFieldHelper>();

        protected override FieldGroupHelper<GameField, GameFieldGroup> RegisterFieldGroupHelper() =>
            TypeHelper.Register<GameFieldGroupHelper>();

        protected override DecoratorFactory<GameField, Game> CreateDecoratorFactory() =>
            new GameDecoratorFactory();

        protected override ControlFactory<GameField, Game> CreateControlFactory() =>
            new GameControlFactory();

        public override FieldSortings<GameField, Game> DefaultSorting() =>
            new()
            {
                new FieldSorting<GameField, Game>(GameField.Name, SortOrder.Ascending)
            };

        public override Categories<GameField, Game> SystemCategories =>
            GameSystemCategories.Categories;

        public override bool UseImageList => true;

        protected override void SetHandlers()
        {
            DataManager.ListController<ConsoleField, PSConsole>().RemoveHandler += OnConsoleRemove;
            DataManager.ListController<AccountField, Account>().RemoveHandler += OnAccountRemove;
        }

        private void OnAccountRemove(Account dao, DAOEntityEventArgs e)
        {
            foreach (Game game in FullItemsList.FindAll((g) => g.Owner == dao.Id))
                game.Owner = Guid.Empty;
        }

        private void OnConsoleRemove(PSConsole dao, DAOEntityEventArgs e)
        {
            List<Game> gamesInstalledOnCurrentConsole = FullItemsList
                .FindAll(
                    (g) => g.Installations.Contains(
                        (i) => i.ConsoleId == dao.Id
                    )
                );

            foreach (Game game in gamesInstalledOnCurrentConsole)
                game.Installations.RemoveAll((i) => i.ConsoleId == dao.Id);
        }

        protected override Bitmap? GetIcon() => OxIcons.Game;

        public override List<ToolStripMenuItem>? MenuItems(Game? item)
        {
            if (item == null)
                return null;

            List<ToolStripMenuItem> result = new();
            ToolStripMenuItem goToItem = new("Go to", OxIcons.Go);

            foreach (Link<GameField> link in item.Links)
                goToItem.DropDownItems.Add(new ItemsRootGridLinkToolStripMenuItem<GameField>(link));

            result.Add(goToItem);
            return result;
        }

        public override string GetExtractItemCaption(GameField field, object? value)
        {
            if (field == GameField.Owner && value is Guid guidValue)
            {
                Account? account = DataManager.Item<AccountField, Account>(AccountField.Id, guidValue);
                return account != null ? account.Name : "Blank";
            }

            return field switch
            {
                GameField.Installed => 
                    value is bool boolValue && boolValue
                        ? "Installed" 
                        : "Not installed",
                GameField.Licensed => 
                    value is bool boolValue && boolValue
                        ? "Licensed" 
                        : "Unlicense",
                GameField.ExistsDLCsWithTrophyset =>
                    "Games with additional trophies",
                GameField.Verified =>
                    "Verified",
                GameField.AvailablePlatinum =>
                    "Platinum Availble",
                GameField.SinglePlayer =>
                    "Single player",
                GameField.CoachMultiplayer =>
                    "Coach multiplayer",
                GameField.OnlineMultiplayer =>
                    "Online multiplayer",
                GameField.Multiplayer =>
                    "Multiplayer",
                _ => 
                    base.GetExtractItemCaption(field, value),
            };
        }
    }
}