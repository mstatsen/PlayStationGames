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
using PlayStationGames.PSNEngine.Data.Types;


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
            TypeHelper.Register<StatusHelper>();
            TypeHelper.Register<LevelValueTypeHelper>();
            TypeHelper.Register<LevelValueTypeGroupHelper>();
            TypeHelper.Register<PegiHelper>();
            TypeHelper.Register<PlatformFamilyHelper>();
            TypeHelper.Register<PlatformTypeHelper>();
            TypeHelper.Register<PlayModeGroupHelper>();
            TypeHelper.Register<PlayModeHelper>();
            TypeHelper.Register<ScreenViewHelper>();
            TypeHelper.Register<SourceHelper>();
            TypeHelper.Register<TrophysetAccessHelper>();
            TypeHelper.Register<TrophyTypeHelper>();
            TypeHelper.Register<GameRegionHelper>();
            TypeHelper.Register<GameLanguageHelper>();
            TypeHelper.Register<GameLinkTypeHelper>();
        }

        public override string Name => "Games";

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

        public override List<ISummaryPanel>? GeneralSummaries => new()
        {
            new GamesSummary()
        };

        public override bool UseImageList => true;
    }
}